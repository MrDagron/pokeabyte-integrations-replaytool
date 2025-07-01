using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Timers;
using BizHawk.Common;
using Newtonsoft.Json;
using PokeAByte.Integrations.ReplayTool.Logic.Helpers;
using PokeAByte.Integrations.ReplayTool.Models;
using Timer = System.Timers.Timer;

namespace PokeAByte.Integrations.ReplayTool.Logic.Services;
//https://github.com/endel/FossilDelta/
/*
 * TODO:
 * AddSaveRange (*)
 * UpdateSave
 * DeleteSave
 *
 * (*) Need to think about if I want to include these here since they require the MainForm or if I want to
 * further decouple this service from the GUI layer
 */
public class SaveStateService
{
    private SaveStateModel? _saveStateModel;
    private readonly ConcurrentQueue<SaveState> _saveStateQueue = new();
    private byte[] _lastState = [];
    private int _currentSize = 0;
    private bool _isSaving = false;
    private string _saveFilePath = "";
    private bool _shouldSave = false;
    //temp, allow user to choose or make it logical
    private double _keyframePercent = 0.05;
    
    private Thread _workerThread;
    private CancellationTokenSource _cancellationTokenSource;
    
    public SaveStateService()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _workerThread = new Thread(() => 
            ProcessSaveStates(_cancellationTokenSource.Token))
        {
            IsBackground = true,
            Name = "ProcessSaveStateQueueThread"
        };
        _workerThread.Start();
    }

    private void ProcessSaveStates(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            //Todo: debug, remove
            HandleSaveStateQueue();
            
            Thread.Sleep(20);
        }
    }
    
    private void HandleSaveStateQueue()
    {
        if (_saveStateQueue.Count == 0 || 
            _lastState.Length == 0 ||
            _saveStateModel is null)
        {
            //Todo: handle this
            return;
        }
        
        if (!_saveStateQueue.TryDequeue(out var saveState))
        {
            //Todo: handle this
            return;
        }
        
        var stateBytes = saveState.FullState;
        //get the delta between the current state and the last state
        var delta = Fossil.Delta.Create(_lastState, stateBytes);
        //Todo: figure out how we should handle the error
        if (delta is null || delta.Length == 0)
        {
            Log.Error(nameof(SaveStateService), 
                "Failed to get delta between states"); 
        }
        else
        {
            //compress the delta further
            saveState.StateDelta = ZStdHelpers.Compress(delta);
            /*saveState.StateDeltaSize = delta.Length;
            saveState.CompressedDeltaSize = saveState.StateDelta.Length;*/
            //clear the full state to reduce memory usage
            saveState.FullState = [];
            _saveStateModel.SaveStates.Add(saveState);
            //Update the last state
            _lastState = stateBytes;
            _shouldSave = true; 
        }
    }
    
    #region SaveState Methods
    public void SaveState(int frame, 
        byte[] state, 
        long saveTimeMs,
        bool isFlagged = false, 
        string flagName = "")
    {
        //Since we want to be on a different thread, let's make sure we make a local clone
        //of the state before we do anything else
        if (state.Clone() is not byte[] stateBytes)
        {
            throw new NullReferenceException("State is null");
        }
        //If the _saveStateModel is not created then this should be the 
        //very first state created
        if (_saveStateModel == null)
        {
            _lastState = state;
            _saveStateModel = new SaveStateModel
            {
                FirstState = stateBytes,
                SaveStates = [],
            };
        }
        _saveStateQueue.Enqueue(new SaveState
        {
            Key = _currentSize,
            Frame = frame,
            FlagName = flagName,
            IsFlagged = isFlagged,
            SaveTimeMs = saveTimeMs,
            StateDelta = [],
            FullState = stateBytes,
        });
        _currentSize += 1;
    }
    
    public void BuildKeyframes()
    {
        if (_saveStateModel is null)
        {
            return;
        }
        var keyframeCount = (int)Math.Ceiling(_saveStateModel.SaveStates.Count * _keyframePercent);
        var interval = Math.Max(1, _saveStateModel.SaveStates.Count / keyframeCount);
        var lastState = _saveStateModel.FirstState;
        foreach (var saveState in _saveStateModel.SaveStates)
        {
            var reconstructedState = ReconstructState(saveState.StateDelta, lastState);
            if (reconstructedState.Length == 0)
            {
                return;
            }
            if (saveState.Key % interval == 0)
            {
                saveState.FullState = reconstructedState;
                saveState.IsKeyframe = true;
            }
            lastState = reconstructedState;
        }
        _saveStateModel.HasBeenReconstructed = true;
    }

    private byte[] ReconstructState(byte[] stateDelta, byte[] lastState)
    {
        var delta = ZStdHelpers.Decompress(stateDelta);
        if (delta.Length == 0)
        {
            //Todo: figure out how we should handle this
            Log.Error(nameof(SaveStateService), 
                "Failed to get delta between states");
            return [];
        }
        var reconstructedState = Fossil.Delta.Apply(lastState, delta);
        if (reconstructedState is null || reconstructedState.Length == 0)
        {
            //Todo: figure out how we should handle this
            Log.Error(nameof(SaveStateService), 
                "Failed to reconstruct state");
            return [];
        }
        return reconstructedState;
    }
    public byte[]? GetReconstructedState(int key)
    {
        if (_saveStateModel is null)
        {
            throw new NullReferenceException("SaveStateModel is null");
        }

        if (key < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(key), "Key must be greater than 0");
        }

        if (_saveStateModel.HasBeenReconstructed == false)
        {
            //try to reconstruct the saves once just to be safe
            BuildKeyframes();
            //If it still isn't reconstructed, then just throw an exception 
            if (_saveStateModel.HasBeenReconstructed == false)
            {
                throw new InvalidOperationException("Failed to reconstruct save states");
            }
        }
        var saveState = _saveStateModel.SaveStates.FirstOrDefault(s => s.Key == key);
        if (saveState is null)
        {
            throw new InvalidOperationException($"Failed to find save state for key {key}");
        }

        //This frame is a keyframe, return it
        if (saveState.IsKeyframe)
        {
            return saveState.FullState;
        }
        
        //backtrack to the last keyframe closest to this key
        var lastKeyframe = _saveStateModel.SaveStates
            .LastOrDefault(s => s.IsKeyframe && s.Key <= key);
        if (lastKeyframe is null)
        {
            //todo: think about this more
            throw new InvalidOperationException($"Failed to find last keyframe for key {key}");
        }
        return ReconstructToKey(saveState.Key, lastKeyframe);
    }

    private byte[] ReconstructToKey(int key, SaveState startState)
    {
        if (_saveStateModel is null)
        {
            return [];
        }
        var lastState = startState.FullState;
        foreach (var saveState in _saveStateModel.SaveStates.Where(s => s.Key > startState.Key && s.Key <= key))
        {
            var currentState = ReconstructState(saveState.StateDelta, lastState);
            if (currentState.Length == 0)
            {
                return [];
            }
            lastState = currentState;
        }
        return lastState;
    }
    #endregion
    
    #region File Methods
    //Todo: probably switch to memory mapped files to save instead of constantly deleting and resaving the file
    public void SaveToFile(string path)
    {
        if (_isSaving || !_shouldSave)
        {
            return;
        }
        _isSaving = true;

        try
        {
            var serializeMessage = SerializationHelper.SerializeJsonToFile(_saveStateModel, path);
            if (!string.IsNullOrEmpty(serializeMessage))
            {
                Log.Error(nameof(SaveStateService), 
                    "Failed to save to file: {serializeMessage}", 
                    serializeMessage);
            }
        }
        catch (Exception e)
        {
            Log.Error(nameof(SaveStateService), 
                "Failed to save to file: {e}",
                e);
        }
        
        _isSaving = false;
    }

    public void LoadFromFile(string path)
    {
        if (_isSaving)
        {
            return;
        }
        try
        {
            var deserialized = SerializationHelper.DeserializeJsonFromFile<SaveStateModel>(path);
            if (!string.IsNullOrEmpty(deserialized.Message))
            {
                Log.Error(nameof(SaveStateService), 
                    "Failed to load from file: {deserializeMessage.Item1}", 
                    deserialized.Message);
            }

            else if (deserialized.Data is null)
            {
                Log.Error(nameof(SaveStateService), 
                    "Failed to load from file: deserialized.Data is null");
            }
            else
            {
                _saveStateModel = deserialized.Data;
            }
        }
        catch (Exception e)
        {
            Log.Error(nameof(SaveStateService), 
                "Failed to load from file: {e}", e);
        }
    }
    #endregion
    
    //debugging helpers
    public int GetKeyframeCount()
    {
        if (_saveStateModel is null)
        {
            return 0;
        }
        return _saveStateModel.SaveStates.Count(s => s.IsKeyframe);
    }

    public int GetReconstructedStatesTotal()
    {
        if (_saveStateModel is null)
        {
            return 0;
        }
        return _saveStateModel.SaveStates.Count(s => s.FullState.Length > 0);
    }

    public int GetStateCount()
    {
        if (_saveStateModel is null)
        {
            return 0;
        }
        return _saveStateModel.SaveStates.Count;
    }
}