using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Timers;
using BizHawk.Common;
using PokeAByte.Integrations.ReplayTool.Models;

namespace PokeAByte.Integrations.ReplayTool.Logic;
//https://github.com/endel/FossilDelta/
/*
 * TODO:
 * GetStateData (*)
 * LoadSaveState (*)
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
    private ConcurrentQueue<SaveState> _saveStateQueue = new();
    private byte[] _lastState = [];
    private int _currentSize = 0;
    private Timer _queueTimer;
    private bool _isSaving = false;
    public SaveStateService()
    {
        InitializeQueueTimer();
    }
    private void InitializeQueueTimer()
    {
        _queueTimer = new Timer();
        _queueTimer.Interval = 16;
        _queueTimer.Elapsed += HandleSaveStateQueue;
        _queueTimer.AutoReset = true;
        _queueTimer.Start();
    }
    private void HandleSaveStateQueue(object sender, ElapsedEventArgs elapsedEventArgs)
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
            return;
        }
        saveState.StateDelta = delta;
        //clear the full state to reduce memory usage
        saveState.FullState = [];
        _saveStateModel.SaveStates.Add(saveState);
        //Update the last state
        _lastState = stateBytes;
    }
    
    public void SaveState(int frame, 
        byte[] state, 
        long saveTimeMs,
        bool isFlagged = false, 
        string flagName = "")
    {
        //If the _saveStateModel is not created then this should be the 
        //very first state created
        if (_saveStateModel == null)
        {
            _lastState = state;
            _saveStateModel = new SaveStateModel
            {
                FirstState = state,
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
            FullState = state,
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

    public void SaveToFile(string path)
    {
        if (_isSaving)
        {
            return;
        }
        _queueTimer.Stop();
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
        _queueTimer.Start();
    }

    public void LoadFromFile(string path)
    {
        if (_isSaving)
        {
            return;
        }
        _queueTimer.Stop();
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
        _queueTimer.Start();
    }
    
 
}