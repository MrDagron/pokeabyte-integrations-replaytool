using System;
using BizHawk.Common;
using PokeAByte.Integrations.ReplayTool.Logic.Helpers;
using System.Timers;

namespace PokeAByte.Integrations.ReplayTool;

//Events that are required for Recording functionality
public partial class ReplayToolForm
{
    private bool _isRecording = false;
    private int _saveStateTimeMs = 1000;
    private Timer _saveStateTimer;
    private bool _shouldSaveState = false;
    private void ConfigureSaveStateTimer()
    {
        _saveStateTimer = new Timer(_saveStateTimeMs);
        _saveStateTimer.AutoReset = false;
        _saveStateTimer.Elapsed += OnSaveStateTimerElapsedHandler;
    }

    private void OnSaveStateTimerElapsedHandler(object sender, ElapsedEventArgs elapsedEventArgs)
    {
        _shouldSaveState = true;
    }

    private void SaveState(bool isFlagged = false, string flagName = "")
    {
        if (!_isRecording || !_shouldSaveState)
        {
            return;
        }
        _saveStateTimer.Stop();
        //todo: get last state size and pass into SaveState
        var savestate = EmulatorHelper.SaveState(PokeAByteMainForm);
        if (savestate is null)
        {
            Log.Error(nameof(SaveState), "Failed to save state");
        }
        else
        {
            //todo: get timer for `saveTimeMs`
            _replayManager.SaveState(savestate.Frame, savestate.SaveState, 0, false, isFlagged, flagName);
        }
        _saveStateTimer.Start();
        _shouldSaveState = false;
    }
    private void recordBtn_Click(object sender, EventArgs e)
    {
        if (!_isRecording)
        {
            recordBtn.Text = "Stop Recording";
            _replayManager.Reset();
            _isRecording = true;
            _saveStateTimer.Start();
        }
        else
        {
            recordBtn.Text = "Start Recording";
            _isRecording = false;
            _saveStateTimer.Stop();
            //save file dlg
            
            //move to playback mode
            _inRecordingMode = false;
            mainFormTabs.SelectedTab = playbackTab;
            ClientSize = new System.Drawing.Size(1255, 528);
        }
    }
    private void recordingPauseEmulatorBtn_Click(object sender, EventArgs e)
    {
        if(PokeAByteMainForm.EmulatorPaused)
        {
            _saveStateTimer.Start();
            recordingPauseEmulatorBtn.Text = "Pause Emulator";
        }
        else
        {
            _saveStateTimer.Stop();
            recordingPauseEmulatorBtn.Text = "Resume Emulator";
        }
        PokeAByteMainForm.TogglePause();
    }
}