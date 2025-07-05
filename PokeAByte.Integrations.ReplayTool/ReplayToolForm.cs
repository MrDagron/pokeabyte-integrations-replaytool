using System;
using System.Drawing;
using BizHawk.Client.Common;
using BizHawk.Client.EmuHawk;
using BizHawk.Common;
using PokeAByte.Integrations.ReplayTool.Logic.Services;
using PokeAByte.Integrations.ReplayTool.Models;

namespace PokeAByte.Integrations.ReplayTool;

[ExternalTool("PokeAByte.Integrations.ReplayTool")]
public sealed partial class ReplayToolForm : ToolFormBase, IExternalToolForm
{

    protected override string WindowTitleStatic => "PokeAByte Replay Tool";
    
    private MainForm PokeAByteMainForm => (MainForm)MainForm;
    
    private readonly SaveStateService _saveStateService;
    private readonly TcpServerService _tcpServerService;
    private readonly RecordingSettings _recordingSettings;
    private bool _inRecordingMode = true;
    public ReplayToolForm()
    {
        //todo: read from settings files
        _recordingSettings = new RecordingSettings();
        _saveStateService = new SaveStateService(_recordingSettings);
        ConfigureSaveStateTimer();
        
        Closing += (_, _) =>
        {
            Cleanup();
        };
        
        InitializeComponent();
        ClientSize = new Size(868, 96);
        mainFormTabs.ItemSize = new Size(0, 1);
        //remove tab bar
        /*mainFormTabs.Top -= mainFormTabs.ItemSize.Height;
        mainFormTabs.Height += mainFormTabs.ItemSize.Height;
        mainFormTabs.Region = new Region(
            new RectangleF(
                recordingTab.Left,
                recordingTab.Top, 
                recordingTab.Width,
                recordingTab.Height + mainFormTabs.ItemSize.Height));*/
        StartServer();
        //todo: connection settings?
        _tcpServerService = new TcpServerService
        {
            OnMessageReceived = OnTcpServerMessage,
            OnConnected = OnTcpServerConnected,
            OnDisconnected = OnTcpServerDisconnected
        };
        _tcpServerService.Start();
    }

    public override void Restart() {
        // executed once after the constructor, and again every time a rom is loaded or reloaded
        EDPSRestart();
    }
    protected override void UpdateAfter() {
        // executed after every frame (except while turboing, use FastUpdateAfter for that)
        SaveState();
        if (this.MemoryDomains != null)
        {   
            this._processor?.Update(this.MemoryDomains);
        }
    }
    //temp, find a better place for this
    private void OnTcpServerMessage(string message)
    {
        //debug code, change later
        Log.Error("", message);
    }

    private void OnTcpServerConnected(string connectedTo)
    {
        tcpServerLabel.Text = $"Overlay Connected: {connectedTo}";
    }

    private void OnTcpServerDisconnected()
    {
        tcpServerLabel.Text = "Waiting for overlay...";
    }
    //Temp, remove
    private void doStuffBtn_Click(object sender, EventArgs e)
    {
        /*_fpsCounter = 0;
        _fpsSum = 0;
        _fpsAvg = 0;*/
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