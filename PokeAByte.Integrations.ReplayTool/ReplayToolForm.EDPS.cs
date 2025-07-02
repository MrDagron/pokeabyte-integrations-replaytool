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
    private void EDPSRestart()
    {
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
}