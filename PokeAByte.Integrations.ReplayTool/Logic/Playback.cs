using System;
using System.Collections.Generic;
using System.Linq;
using PokeAByte.Integrations.ReplayTool.Logic.Helpers;
using PokeAByte.Integrations.ReplayTool.Models;

namespace PokeAByte.Integrations.ReplayTool.Logic;

public sealed class Playback
{
    private List<PlaybackState> _playbackStates = [];
    private Keyframe? _lastKeyframe = null;
    
    public void SetPlaybackStates(PlaybackState[] playbackStates)
    {
        _playbackStates = new List<PlaybackState>(playbackStates);
    }
    
    public byte[] GetReconstructedState(int frame)
    {
        //todo: better handling 
        if (_playbackStates.Count == 0 || frame < 0)
        {
            return [];
        }
        //find the frame we want
        var saveState = _playbackStates.FirstOrDefault(p => p.Frame == frame);
        //Todo: better handling
        if (saveState is null)
        {
            return [];
        }

        if (saveState.IsKeyframe)
        {
            //we already have the data reconstructed, just return it
            return saveState.SaveState;
        }
        
        var lastKeyframeState = _playbackStates
            .LastOrDefault(s => s.IsKeyframe && s.Frame <= frame);
        //We shouldn't hit this because we are always setting the first state to be a keyframe but lets be safe
        if (lastKeyframeState is null)
        {
            throw new InvalidOperationException($"Failed to find last keyframe for key {frame}");
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
        var reconstructedState = ReconstructToFrame(frame, lastKeyframe);
        //cache the last keyframe
        _lastKeyframe = new Keyframe
        {
            Frame = frame,
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
}