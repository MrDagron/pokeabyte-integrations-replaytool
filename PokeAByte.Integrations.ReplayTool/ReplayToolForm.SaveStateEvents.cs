using System;
using System.IO;
using System.Reflection;
using System.Timers;
using BizHawk.Common;
using PokeAByte.Integrations.ReplayTool.Logic.Helpers;

namespace PokeAByte.Integrations.ReplayTool;

//Events that are required for SaveState functionality
public partial class ReplayToolForm
{
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
            _saveStateService.SaveState(savestate.Frame, savestate.SaveState, 0, isFlagged, flagName);
        }
        _saveStateTimer.Start();
        _shouldSaveState = false;
    }
}