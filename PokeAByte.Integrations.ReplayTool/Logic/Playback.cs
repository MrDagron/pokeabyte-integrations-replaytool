using System;
using System.Collections.Generic;
using System.Linq;
using PokeAByte.Integrations.ReplayTool.Logic.Helpers;
using PokeAByte.Integrations.ReplayTool.Models;

namespace PokeAByte.Integrations.ReplayTool.Logic;

public sealed class Playback
{
    private PlaybackState[] _playbackStates = [];
    private Keyframe? _lastKeyframe = null;
    
    public void SetPlaybackStates(PlaybackState[] playbackStates)
    {
        _playbackStates = playbackStates;
    }
    
    public byte[] GetReconstructedState(int key)
    {
        //todo: better handling 
        if (_playbackStates.Length == 0 || key < 0 || key >= _playbackStates.Length)
        {
            return [];
        }
        //find the frame we want
        var saveState = _playbackStates[key];/*_playbackStates.FirstOrDefault(p => p.Frame == frame);*/

        if (saveState.IsKeyframe)
        {
            //we already have the data reconstructed, just return it
            return saveState.SaveState;
        }
        
        var lastKeyframeState = _playbackStates
            .LastOrDefault(s => s.IsKeyframe && s.Frame <= saveState.Frame);
        //We shouldn't hit this because we are always setting the first state to be a keyframe but lets be safe
        if (lastKeyframeState is null)
        {
            throw new InvalidOperationException($"Failed to find last keyframe for key {saveState.Frame}");
        }

        var lastKeyframe = new Keyframe
        {
            Frame = lastKeyframeState.Frame,
            SaveState = lastKeyframeState.SaveState
        };
        
        //Check to see if the cached frame is closer
        if (_lastKeyframe is not null && 
            _lastKeyframe.Frame - saveState.Frame  < lastKeyframe.Frame - saveState.Frame )
        {
            lastKeyframe = _lastKeyframe;
        }
        var reconstructedState = ReconstructToFrame(saveState.Frame, lastKeyframe);
        //cache the last keyframe
        _lastKeyframe = new Keyframe
        {
            Frame = saveState.Frame,
            SaveState = reconstructedState
        };
        return reconstructedState;
    }

    private byte[] ReconstructToFrame(int frame, Keyframe startState)
    {
        var lastState = startState.SaveState;
        foreach (var saveState in _playbackStates
                     .Where(s => s.Frame > startState.Frame && s.Frame <= frame))
        {
            var currentState = ReconstructState(saveState.SaveState, lastState);
            if (currentState.Length == 0)
            {
                //todo: handle this
                return [];
            }
            lastState = currentState;
        }
        return lastState;
    }

    private byte[] ReconstructState(byte[] stateDelta, byte[] fullState)
    {
        var decompressedDelta = ZStdHelpers.Decompress(stateDelta);
        if (decompressedDelta.Length == 0)
        {
            //todo: handle this
            return [];
        }
        var reconstructedState = Fossil.Delta.Apply(fullState, decompressedDelta);
        if (reconstructedState is null || reconstructedState.Length == 0)
        {
            //todo: handle this
            return [];
        }
        return reconstructedState;
    }

    public int GetPlaybackStateCount() => _playbackStates.Length;
}