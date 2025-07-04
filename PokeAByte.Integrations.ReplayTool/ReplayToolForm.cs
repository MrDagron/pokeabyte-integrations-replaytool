using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

using BizHawk.Client.Common;
using BizHawk.Client.EmuHawk;
using BizHawk.Common;
using BizHawk.Emulation.Common;
using PokeAByte.Integrations.ReplayTool.Logic.Helpers;
using PokeAByte.Integrations.ReplayTool.Logic.Services;
using PokeAByte.Protocol;
using PokeAByte.Protocol.BizHawk;
using PokeAByte.Protocol.BizHawk.PlatformData;

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
        
        Closing += (_, _) =>
        {
            Cleanup();
        };
        
        InitializeComponent();
        
        StartServer();
    }

    public override void Restart() {
        // executed once after the constructor, and again every time a rom is loaded or reloaded
        EDPSRestart();
    }
    //tmp remove
    private int fpsSum = 0;
    private int fpsAvg = 0;
    private int count = 0;
    private int low = 1000;
    private int high = 0;
    protected override void UpdateAfter() {
        // executed after every frame (except while turboing, use FastUpdateAfter for that)
        SaveState();
        if (this.MemoryDomains != null)
        {   
            this._processor?.Update(this.MemoryDomains);
        }

        count++;
        fpsSum += PokeAByteMainForm.GetApproxFramerate();
        fpsAvg = fpsSum / count;
        if (fpsAvg > high)
        {
            high = fpsAvg;
        }

        if (fpsAvg < low)
        {
            low = fpsAvg;
        }
        fpsLabel.Text = $"FPS - Avg: {fpsAvg}, Low: {low}, High: {high}, Count: {count}";
    }
    //Temp, remove
    private void doStuffBtn_Click(object sender, EventArgs e)
    {
        count = 0;
        fpsAvg = 0;
        fpsSum = 0;
        low = 1000;
        high = 0;
        /*if (_isRecording)
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
        }*/
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