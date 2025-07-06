using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BizHawk.Common;

namespace PokeAByte.Integrations.ReplayTool.Logic;

public delegate void ServerConnected(string connectedTo);
public delegate void ServerDisconnected();
public delegate void HandleClientMessage(string message);
public sealed class TcpServer
{
    private readonly IPEndPoint _endpoint;
    private TcpListener _listener;    
    private Task? _serverTask;
    private CancellationTokenSource? _cancellationTokenSource;
    private bool _isRunning = false;
    
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
        _isRunning = false;
        _cancellationTokenSource?.Cancel();
        _serverTask = null;
    }
    public void StartServer()
    {
        if (_serverTask != null) return;
        _cancellationTokenSource = new CancellationTokenSource();
        _serverTask = Task.Run(async () =>
        {
            try
            {
                _listener = new TcpListener(_endpoint);
                _listener.Start();
                _isRunning = true;
            
                while (_isRunning)
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
        }, _cancellationTokenSource.Token);
    }
    
    private async Task HandleClientAsync(TcpClient client)
    {
        try
        {
            using (client)
            using (NetworkStream stream = client.GetStream())
            {
                var buffer = new byte[1024];
                var partialMessage = string.Empty;
                // Keep reading messages until client disconnects
                while (client.Connected && _isRunning)
                {
                    try
                    {
                        var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                        
                        if (bytesRead == 0)
                        {
                            // Client has disconnected
                            break;
                        }
                        
                        var message = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                        //Check if we were sent a new line
                        if (message.IndexOf('\n') != -1)
                        {
                            //Concat the partial message on the message and split on new lines
                            var splitMessage = (partialMessage + message).Split('\n');
                            //iterate through all new lines 
                            foreach (var m in splitMessage)
                            {
                                OnMessageReceived?.Invoke(m);
                            }
                            //Store the last message
                            partialMessage = splitMessage[^1];
                        }
                        else
                        {
                            //No new line, just add onto the end of the message buffer
                            partialMessage += message;
                        }
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
