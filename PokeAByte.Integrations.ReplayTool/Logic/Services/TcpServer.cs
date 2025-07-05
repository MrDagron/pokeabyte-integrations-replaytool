using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using BizHawk.Common;

namespace PokeAByte.Integrations.ReplayTool.Logic.Services;

public delegate void ServerConnected(string connectedTo);
public delegate void ServerDisconnected();
public delegate void HandleClientMessage(string message);
public sealed class TcpServer
{
    private IPEndPoint _endpoint;
    private TcpListener _listener;
    public bool IsRunning { get; private set; }
    
    public HandleClientMessage? OnMessageReceived { get; set; }
    public ServerConnected? OnConnected { get; set; }
    public ServerDisconnected? OnDisconnected { get; set; }
    private readonly string _ipAddress;
    private readonly string _port;
    public TcpServer(string host = "127.0.0.1", int port = 4520)
    {
        _ipAddress = host;
        _port = port.ToString();
        var ip = IPAddress.Parse(host);
        _endpoint = new IPEndPoint(ip, port);
    }

    public void StopServer()
    {
        IsRunning = false;
    }
    public async Task StartServer()
    {
        try
        {
            _listener = new TcpListener(_endpoint);
            _listener.Start();
            IsRunning = true;
            
            while (IsRunning)
            {
                var client = await _listener.AcceptTcpClientAsync();
                OnConnected?.Invoke($"{_ipAddress}:{_port}");
                
                // Handle each client in a separate task
                _ = Task.Run(() => HandleClientAsync(client));
            }
        }
        catch (Exception e)
        {
            Log.Error("TCP", $"Failed to start server: {e.Message}");
        }
        finally
        {
            _listener?.Stop();
        }
    }
    
    private async Task HandleClientAsync(TcpClient client)
    {
        try
        {
            using (client)
            using (NetworkStream stream = client.GetStream())
            {
                var buffer = new byte[1024];
                
                // Keep reading messages until client disconnects
                while (client.Connected && IsRunning)
                {
                    try
                    {
                        // Set read timeout to avoid hanging indefinitely
                        var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                        
                        if (bytesRead == 0)
                        {
                            // Client has disconnected
                            break;
                        }
                        
                        var message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                        OnMessageReceived?.Invoke(message);
                    }
                    catch (Exception)
                    {
                        // Client disconnected
                        break;
                    }
                }
            }
            Log.Error("TCP", "Client disconnected");
            OnDisconnected?.Invoke();
        }
        catch (Exception ex)
        {
            Log.Error("TCP", $"Failed to handle client: {ex.Message}");
        }
    }
}
