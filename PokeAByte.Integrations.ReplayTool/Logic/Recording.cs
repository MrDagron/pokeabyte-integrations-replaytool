﻿using System;
using System.Collections.Generic;
using PokeAByte.Integrations.ReplayTool.Logic.Helpers;
using PokeAByte.Integrations.ReplayTool.Models;

namespace PokeAByte.Integrations.ReplayTool.Logic;

public sealed class Recording
{
    private byte[] _firstState = [];
    private List<RecordedSaveState> _recordedSaveStates = [];
    private byte[] _lastState = [];
    private List<int> _keyframes = [];

    public bool HasInitialState => _firstState.Length > 0;

    public void SetFirstState(byte[] state)
    {
        _firstState = state;
        _lastState = state;
    }
    
    public string Add(RecordedSaveState state, int keyframeInterval)
    {
        var stateBytes = state.FullState;
        var delta = Fossil.Delta.Create(_lastState, stateBytes);            
        //Todo: figure out how we should handle the error
        if (delta is null || delta.Length == 0)
        {
            throw new InvalidOperationException("Failed to get delta between states");
        }
        //compress the delta further
        state.StateDelta = ZStdHelpers.Compress(delta);
        if (state.StateDelta.Length == 0)
        {
            throw new InvalidOperationException("Failed to compress delta");       
        }
        _recordedSaveStates.Add(state);       
        //Check if we should create a keyframe. We should force the first state to be a keyframe
        if (_recordedSaveStates.Count % keyframeInterval == 0 || _recordedSaveStates.Count == 1)
        {
            state.IsKeyframe = true;
            _keyframes.Add(state.Frame);
        }
        else
        {
            //clear the full state to reduce memory usage
            state.FullState = [];
        }
        _lastState = stateBytes;
        return $"State #{_recordedSaveStates.Count} - {state}";
    }

    public void Complete()
    {
        _recordedSaveStates.Sort((a, b) => a.Frame.CompareTo(b.Frame));
    }

    public PlaybackState[] GetAsPlaybackStateArray()
    {
        var result = new PlaybackState[_recordedSaveStates.Count];
        for (var i = 0; i < result.Length; i++)
        {
            var recordedState = _recordedSaveStates[i];
            result[i] = new PlaybackState
            {
                Frame = recordedState.Frame,
                IsFlagged = recordedState.IsFlagged,
                FlagName = recordedState.FlagName,
                IsKeyframe = recordedState.IsKeyframe,
                SaveState = recordedState.IsKeyframe ? 
                    recordedState.FullState : 
                    recordedState.StateDelta
            };
        }
        return result;
    }

    public void Reset()
    {
        _firstState = [];
        _recordedSaveStates = [];
        _lastState = [];
        _keyframes = [];
    }
    
    public ReplayFile SaveAsReplayFile()
    {
        return new ReplayFile
        {
            FirstState = _firstState,
            States = _recordedSaveStates,
            Keyframes = _keyframes
        };
    }
}