using System;
using System.Linq;
using BizHawk.Client.Common;
using BizHawk.Emulation.Common;
using PokeAByte.Protocol;
using PokeAByte.Protocol.BizHawk;
using PokeAByte.Protocol.BizHawk.PlatformData;

namespace PokeAByte.Integrations.ReplayTool;

public partial class ReplayToolForm
{    
    public ApiContainer? APIs { get; set; }

    [RequiredService]
    public IMemoryDomains MemoryDomains { get; set; } = null!;
    
    private EmulatorProtocolServer? _server;
    private string _initializedGame = "";
    private GameDataProcessor? _processor;
    
    private void StartServer()
    {
        _server = new EmulatorProtocolServer
        {
            OnWrite = (instruction) => this._processor?.QueueWrite(instruction),
            OnSetup = Setup,
            OnCloseRequest = () =>
            {
                EDPSCleanup();
                StartServer();
            }
        };
        _server.Start();
    }

    private void EDPSCleanup()
    {
        EDPSLabel.Text = $"Waiting for Client...";
        _server?.Dispose();
        _server = null;
        _processor?.Dispose();
        _processor = null;
    }

    private void Setup(SetupInstruction instruction)
    {
        if (_processor != null)
        {
            _processor.Dispose();
            _processor = null;
        }
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

    /*private void WriteToMemory(WriteInstruction instruction)
    {
        this._processor?.WriteToMemory(instruction, this.MemoryDomains);
    }*/
    
    private void EDPSRestart()
    {
        var gameInfo = APIs?.Emulation.GetGameInfo();
        var gameIdentifier = gameInfo != null
            ? gameInfo.Name + gameInfo.Hash
            : null;
        if (gameIdentifier != this._initializedGame)
        {
            EDPSCleanup();
            StartServer();
            EDPSLabel.Text = gameInfo == null
                ? "No game is loaded, doing nothing."
                : $"Waiting for client...";
        }
    }
}