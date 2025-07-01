using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

using BizHawk.Client.Common;
using BizHawk.Client.EmuHawk;
using BizHawk.Common;
using PokeAByte.Integrations.ReplayTool.Logic.Helpers;
using PokeAByte.Integrations.ReplayTool.Logic.Services;

namespace PokeAByte.Integrations.ReplayTool;

[ExternalTool("PokeAByte.Integrations.ReplayTool")]
public sealed partial class ReplayToolForm : ToolFormBase, IExternalToolForm
{
    protected override string WindowTitleStatic => "PokeAByte Replay Tool";
    
    private MainForm PokeAByteMainForm => (MainForm)MainForm;
    
    private readonly SaveStateService _saveStateService;
    public ReplayToolForm()
    {
        _saveStateService = new SaveStateService();
        ConfigureSaveStateTimer();
        
        //temp testing
        /*_isRecording = true;
        */
        //_isPlayback = true;
        //end temp testing
        
        InitializeComponent();
    }
    public override void Restart() {
        // executed once after the constructor, and again every time a rom is loaded or reloaded

    }
    protected override void UpdateAfter() {
        // executed after every frame (except while turboing, use FastUpdateAfter for that)
        SaveState();

        /*var state = _saveStateService.GetReconstructedState(test++);
        if (state is not null)
            EmulatorHelper.LoadStateBinary(PokeAByteMainForm, state);*/
    }

    //Temp, remove
    private void doStuffBtn_Click(object sender, EventArgs e)
    {
        if (_isRecording)
        {
            _isRecording = false;
            _saveStateTimer?.Stop();
            doStuffBtn.Text = "Do stuff";
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string assemblyDirectory = Path.GetDirectoryName(assemblyLocation);
            if (!string.IsNullOrWhiteSpace(assemblyDirectory))
            {
                assemblyDirectory = assemblyDirectory.Substring(0, assemblyDirectory.LastIndexOf('\\'));
            }

            var path = assemblyDirectory ?? "";
            path += "\\saveStates.json";
            Log.Error("", $"Saving to {path}");
            _saveStateService.SaveToFile(path);
        }
        else
        {
            _isRecording = true;
            _saveStateTimer?.Start();
            Log.Error("", "Start recording");
            doStuffBtn.Text = "Stop doing stuff";
        }
        //
        /*_saveStateService.LoadFromFile(path);
        _saveStateService.ReconstructSaveStates();
        
        Log.Error("", $"Total States: {_saveStateService.GetStateCount()}");
        Log.Error("", $"Total Keyframes: {_saveStateService.GetKeyframeCount()}");
        Log.Error("", $"Total Reconstructed States: {_saveStateService.GetReconstructedStatesTotal()}");*/
        
        /*var state = _saveStateService.GetReconstructedState(813);
        if (state is not null)
            EmulatorHelper.LoadStateBinary(PokeAByteMainForm, state);*/
        /*PokeAByteMainForm.InvisibleEmulation = true;
        while (PokeAByteMainForm.Emulator.Frame < 10000)
        {
            PokeAByteMainForm.SeekFrameAdvance();
        }
        
        PokeAByteMainForm.InvisibleEmulation = false;
        PokeAByteMainForm.UnpauseEmulator();*/
    }
}