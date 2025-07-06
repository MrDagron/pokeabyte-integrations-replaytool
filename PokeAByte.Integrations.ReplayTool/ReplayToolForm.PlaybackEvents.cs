using System;
using BizHawk.Common;
using PokeAByte.Integrations.ReplayTool.Logic.Helpers;
using PokeAByte.Integrations.ReplayTool.UI;

namespace PokeAByte.Integrations.ReplayTool;

//Events that are required for Playback functionality
public partial class ReplayToolForm
{
    private bool _isPlayback = false;
    private void UpdatePlaybackScrubberPosition()
    {
        if (_isRecording) return;
        if (_isPlayback && !playbackScrubber.IsMouseDown)
        {
            //get current frame
            var frame = PokeAByteMainForm.Emulator.Frame;
            //get the key
            
            //update the scrubber position
            
        }
    }
    private void OnScrubberPositionChanged(object sender, PositionChangedEventArgs e)
    {
        if (_isRecording)
        {
            return;
        }

        var stateCount = _replayManager.GetFrameCount();
        if (stateCount == 0)
        {
            return;
        }

        var index = playbackScrubber.GetIndex(stateCount);
        if (index >= stateCount)
        {
            return;
        }
        
        var state = _replayManager.GetReconstructedState(index);
        if (state is not null)
        {
            EmulatorHelper.LoadStateBinary(PokeAByteMainForm, state);
        }
    }
}