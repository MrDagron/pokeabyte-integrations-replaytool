using System.Threading;
using System.Threading.Tasks;
using BizHawk.Common;

namespace PokeAByte.Integrations.ReplayTool.Logic.Services;
public class TcpServerService
{
    private readonly TcpServer _server;
    private Task? _serverTask;
    private CancellationTokenSource? _cancellationTokenSource;

    public ServerConnected? OnConnected
    {
        get => _server.OnConnected;
        set => _server.OnConnected = value;
    }
    public ServerDisconnected? OnDisconnected
    {
        get => _server.OnDisconnected;
        set => _server.OnDisconnected = value;
    }
    public HandleClientMessage? OnMessageReceived
    {
        get => _server.OnMessageReceived;
        set => _server.OnMessageReceived = value;
    }
    
    public TcpServerService(string host = "127.0.0.1", int port = 4520)
    {
        _server = new TcpServer(host, port);
    }

    public void Start()
    {
        if (_serverTask != null) return;
        
        Log.Error(nameof(TcpServerService), $"Starting TCP Server");
        _cancellationTokenSource = new CancellationTokenSource();
        _serverTask = Task.Run(async () =>
        {
            await _server.StartServer();
        }, _cancellationTokenSource.Token);
    }
    
    public void Stop()
    {
        _server.StopServer();
        _cancellationTokenSource?.Cancel();
        _serverTask = null;
    }
}