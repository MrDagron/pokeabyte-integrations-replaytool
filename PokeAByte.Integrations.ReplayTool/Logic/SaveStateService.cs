using System.Collections.Concurrent;
using System.Collections.Generic;
using BizHawk.Common;
using PokeAByte.Integrations.ReplayTool.Models;

namespace PokeAByte.Integrations.ReplayTool.Logic;
//https://github.com/endel/FossilDelta/
public class SaveStateService
{
    private SaveStateModel? _saveStateModel;
    private ConcurrentQueue<SaveState> _saveStateQueue = new();
    private byte[] _lastState = [];
    private int _currentSize = 0;
    
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

    //Todo: create a timer to execute this method
    public void HandleSaveStateQueue()
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
            Log.Error(nameof(SaveStateService), "Failed to get delta between states"); 
            return;
        }
        saveState.StateDelta = delta;
        //clear the full state to reduce memory usage
        saveState.FullState = [];
        _saveStateModel.SaveStates.Add(saveState);
        //Update the last state
        _lastState = stateBytes;
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
                Log.Error(nameof(SaveStateService), "Failed to get delta between states");
                return;
            }
            var reconstructedState = Fossil.Delta.Apply(lastState, delta);
            if (reconstructedState is null || reconstructedState.Length == 0)
            {
                //Todo: figure out how we should handle this
                Log.Error(nameof(SaveStateService), "Failed to reconstruct state");
                return;
            }
            saveState.FullState = reconstructedState;
            lastState = reconstructedState;
        }
        _saveStateModel.HasBeenReconstructed = true;
    }
}