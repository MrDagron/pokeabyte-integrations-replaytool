using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BizHawk.Common;
using PokeAByte.Integrations.ReplayTool.Logic.Helpers;
using PokeAByte.Integrations.ReplayTool.Models;

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
    private readonly RecordingSettings _recordingSettings;
    private SaveStateModel? _saveStateModel;
    private readonly ConcurrentQueue<SaveState> _saveStateQueue = new();
    private byte[] _lastState = [];
    private (int key, byte[] frame) _lastKeyframe;
    private int _currentKey = 0;
    //temp, allow user to choose or make it logical
    private readonly double _keyframePercent = 0.05;
    
    private Thread _workerThread;
    private CancellationTokenSource _cancellationTokenSource;
    
    public SaveStateService(RecordingSettings recordingSettings)
    {
        _recordingSettings = recordingSettings;
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
        
        while (_saveStateQueue.TryDequeue(out var saveState))
        {
            var stateBytes = saveState.FullState;
            //get the delta between the current state and the last state
            var delta = Fossil.Delta.Create(_lastState, stateBytes);
            //Todo: figure out how we should handle the error
            if (delta is null || delta.Length == 0)
            {
                throw new InvalidOperationException("Failed to get delta between states");
            }

            //compress the delta further
            saveState.StateDelta = ZStdHelpers.Compress(delta);
            //clear the full state to reduce memory usage
            saveState.FullState = [];
            _saveStateModel.SaveStates.Add(saveState);
            //Check if we should make a keyframe
            if(saveState.Key % _recordingSettings.KeyframeIntervalCount == 0)
            {
                saveState.IsKeyframe = true;
            }
            //add to our keyframe list
            if (saveState.IsKeyframe)
            {
                _saveStateModel.Keyframes.Add(saveState.Key);
            }

            //Update the last state
            _lastState = stateBytes;
        }
    }
    
    #region SaveState Methods
    public void SaveState(int frame, 
        byte[] state, 
        long saveTimeMs,
        bool isKeyframe = false,
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
            Key = _currentKey,
            Frame = frame,
            FlagName = flagName,
            IsFlagged = isFlagged,
            SaveTimeMs = saveTimeMs,
            StateDelta = [],
            FullState = stateBytes,
            IsKeyframe = isKeyframe
        });
        _currentKey += 1;
    }
    
    private void BuildSaveStates()
    {
        if (_saveStateModel is null)
        {
            return;
        }
        //Check if we already saved which frames should be keyframes
        if (_saveStateModel.Keyframes.Count > 0)
        {
            var lastState = _saveStateModel.FirstState;
            foreach(var saveState in _saveStateModel.SaveStates)
            {
                var reconstructedState = ReconstructState(saveState.StateDelta, lastState);
                if (reconstructedState.Length == 0)
                {
                    return;
                }
                if (_saveStateModel.Keyframes.Contains(saveState.Key))
                {
                    saveState.FullState = reconstructedState;
                    saveState.IsKeyframe = true;
                }
                lastState = reconstructedState;
            }
        }
        else //Otherwise let's automatically build them
        {
            var keyframeList = new List<int>();
            var count = 1;
            var lastState = _saveStateModel.FirstState;
            foreach (var saveState in _saveStateModel.SaveStates)
            {
                var reconstructedState = ReconstructState(saveState.StateDelta, lastState);
                if (reconstructedState.Length == 0)
                {
                    return;
                }
                if (count == _recordingSettings.KeyframeIntervalCount)
                {
                    saveState.FullState = reconstructedState;
                    saveState.IsKeyframe = true;
                    keyframeList.Add(saveState.Key);
                }
                lastState = reconstructedState;
                count += 1;
            }
            //store the keyframes for later
            _saveStateModel.Keyframes = keyframeList;
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
            BuildSaveStates();
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
        var lastKeyframeState = _saveStateModel.SaveStates
            .LastOrDefault(s => s.IsKeyframe && s.Key <= key);
        if (lastKeyframeState is null)
        {
            //todo: think about this more
            throw new InvalidOperationException($"Failed to find last keyframe for key {key}");
        }

        (int key, byte[] frame) lastKeyframe = new(lastKeyframeState.Key, lastKeyframeState.FullState);
        //Check to see if the cached frame is closer
        if (_lastKeyframe.key - saveState.Key  < lastKeyframe.key - saveState.Key )
        {
            lastKeyframe = _lastKeyframe;
        }
        //cache the last keyframe
        _lastKeyframe = new ValueTuple<int, byte[]>(saveState.Key,ReconstructToKey(saveState.Key, lastKeyframe));
        return _lastKeyframe.frame;
    }

    private byte[] ReconstructToKey(int key, (int key, byte[] frame) startState)
    {
        if (_saveStateModel is null)
        {
            return [];
        }
        var lastState = startState.frame;
        foreach (var saveState in _saveStateModel.SaveStates.Where(s => s.Key > startState.key && s.Key <= key))
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
    }

    public void LoadFromFile(string path)
    {
        try
        {
            var deserialized = SerializationHelper.DeserializeJsonFromFile<SaveStateModel>(path);
            if (!string.IsNullOrEmpty(deserialized.Message))
            {
                Log.Error(nameof(SaveStateService), 
                    "Failed to load from file: {deserialized.Message}", 
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
                BuildSaveStates();
            }
        }
        catch (Exception e)
        {
            Log.Error(nameof(SaveStateService), 
                "Failed to load from file: {e}", e);
        }
    }
    #endregion
    public int GetStateCount()
    {
        return _saveStateModel is null ? 0 : _saveStateModel.SaveStates.Count;
    }
    public void Reset()
    {
        _saveStateModel = null;
        //clear the queue
        while (_saveStateQueue.TryDequeue(out _)) ;
        _lastState = [];
        _lastKeyframe = default;
        _currentKey = 0;
    }
}