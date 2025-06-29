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
            saveState.StateDelta = delta;
            saveState.StateDeltaSize = delta.Length;
            saveState.FullStateSize = Fossil.Delta.OutputSize(delta);
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
    
    public void ReconstructSaveStates()
    {
        if (_saveStateModel is null)
        {
            return;
        }
        var lastState = _saveStateModel.FirstState;
        foreach (var saveState in _saveStateModel.SaveStates)
        {
            var delta = saveState.StateDelta;
            if (delta.Length == 0)
            {
                //Todo: figure out how we should handle this
                Log.Error(nameof(SaveStateService), 
                    "Failed to get delta between states");
                return;
            }
            var reconstructedState = Fossil.Delta.Apply(lastState, delta);
            if (reconstructedState is null || reconstructedState.Length == 0)
            {
                //Todo: figure out how we should handle this
                Log.Error(nameof(SaveStateService), 
                    "Failed to reconstruct state");
                return;
            }
            saveState.FullState = reconstructedState;
            lastState = reconstructedState;
        }
        _saveStateModel.HasBeenReconstructed = true;
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
            ReconstructSaveStates();
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
        return saveState.FullState;
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
}