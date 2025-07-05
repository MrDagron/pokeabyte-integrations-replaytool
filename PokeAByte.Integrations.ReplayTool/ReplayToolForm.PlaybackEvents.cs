using System;
using BizHawk.Common;
using PokeAByte.Integrations.ReplayTool.Logic.Helpers;
using PokeAByte.Integrations.ReplayTool.UI;

namespace PokeAByte.Integrations.ReplayTool;

//Events that are required for Playback functionality
public partial class ReplayToolForm
{
/*    private bool _isPlayback = false;
    private void UpdatePlaybackScrubberPosition()
    {
        if (_isRecording) return;
        if (_isPlayback && !timeScrubber.IsMouseDown)
        {
            //get current frame
            var frame = PokeAByteMainForm.Emulator.Frame;
            //get the key
            
            //update the scrubber position
            
        }
    }
    private void OnScrubberPositionChanged(object sender, PositionChangedEventArgs e)
    {
        if (_isRecording || !_isPlayback)
        {
            return;
        }

        var stateCount = _saveStateService.GetStateCount();
        if (stateCount == 0)
        {
            return;
        }

        var index = timeScrubber.GetIndex(stateCount);
        if (index >= stateCount)
        {
            return;
        }
        
        var state = _saveStateService.GetReconstructedState(index);
        if (state is not null)
        {
            EmulatorHelper.LoadStateBinary(PokeAByteMainForm, state);
        }
    }*/
}