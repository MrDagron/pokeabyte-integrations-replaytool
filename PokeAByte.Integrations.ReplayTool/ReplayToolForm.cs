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
    public ApiContainer? APIs { get; set; }

    [RequiredService]
    public IMemoryDomains MemoryDomains { get; set; } = null!;
    protected override string WindowTitleStatic => "PokeAByte Replay Tool";
    
    private MainForm PokeAByteMainForm => (MainForm)MainForm;
    
    private readonly SaveStateService _saveStateService;
    private EmulatorProtocolServer? _server;
    private string _initializedGame = "";
    private GameDataProcessor? _processor;
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
    private void StartServer()
    {
        _server = new EmulatorProtocolServer();
        _server.OnWrite = WriteToMemory;
        _server.OnSetup = Setup;
        _server.Start();
    }
    private void Cleanup()
    {
        EDPSLabel.Text = $"Waiting for Client...";
        _server?.Dispose();
        _server = null;
        _processor?.Dispose();
        _processor = null;
    }
    private void Setup(SetupInstruction instruction)
    {
        var gameInfo = APIs?.Emulation.GetGameInfo();
        var system = gameInfo?.System ?? string.Empty;
        var platform = PlatformConstants.Platforms.SingleOrDefault(x => x.SystemId == system);
        if (platform == null || gameInfo == null)
        {
            EDPSLabel.Text = $"Waiting for game to load";
            return;
        }
        if (_server == null)
        {
            EDPSLabel.Text = $"Failed to initialize properly.";
            return;
        }
        this._initializedGame = gameInfo.Name + gameInfo.Hash;
        this._processor = new GameDataProcessor(
            platform,
            instruction,
            EDPSLabel
        );
    }

    private void WriteToMemory(WriteInstruction instruction)
    {
        if (instruction.Data.Length != 0 && APIs != null)
        {
            try
            {
                APIs.Memory.WriteByteRange(instruction.Address, instruction.Data);
            }
            catch (Exception) { } // Nothing to do, fail silently.
        }
    }
    public override void Restart() {
        // executed once after the constructor, and again every time a rom is loaded or reloaded
        var gameInfo = APIs?.Emulation.GetGameInfo();
        var gameIdentifier = gameInfo != null
            ? gameInfo.Name + gameInfo.Hash
            : null;
        if (gameIdentifier != this._initializedGame)
        {
            Cleanup();
            StartServer();
            EDPSLabel.Text = gameInfo == null
                ? "No game is loaded, doing nothing."
                : $"Waiting for client...";
        }
    }
    protected override void UpdateAfter() {
        // executed after every frame (except while turboing, use FastUpdateAfter for that)
        SaveState();
        if (this.MemoryDomains != null)
        {   
            this._processor?.Update(this.MemoryDomains);
        }
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