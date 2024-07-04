using Microsoft.AspNetCore.SignalR;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace LoadBalancer.Core.Services;

public class LoadBalancer : ILoadBalancer
{
    private readonly List<IBackendServerCommunicator> _servers;
    private int _currentIndex;
    private readonly TimeSpan _healthCheckInterval;
    private readonly IHubContext<ServerStatusHub> _hubContext;

    public LoadBalancer(List<IBackendServerCommunicator> servers, TimeSpan healthCheckInterval, IHubContext<ServerStatusHub> hubContext)
    {
        _servers = servers;
        _currentIndex = 0;
        _healthCheckInterval = healthCheckInterval;
        _hubContext = hubContext;
    }

    public async Task StartAsync()
    {
        _ = Task.Run(HealthCheckLoop);

        var listener = new TcpListener(IPAddress.Any, 90);
        listener.Start();
        Console.WriteLine("Load balancer started on port 90...");

        while (true)
        {
            var client = await listener.AcceptTcpClientAsync();
            _ = HandleClientAsync(client);
        }
    }

    private async Task HandleClientAsync(TcpClient client)
    {
        using var stream = client.GetStream();
        var buffer = new byte[1024];
        var length = await stream.ReadAsync(buffer);
        var requestString = Encoding.UTF8.GetString(buffer, 0, length);
        Console.WriteLine($"Received request:\n{requestString}");

        var healthyServers = _servers.Where(s => s.IsHealthy).ToList();
        if (!healthyServers.Any())
        {
            var response = Encoding.UTF8.GetBytes("HTTP/1.1 503 Service Unavailable\r\n\r\nNo healthy servers available.");
            await stream.WriteAsync(response, 0, response.Length);
            client.Close();
            return;
        }

        var server = healthyServers[_currentIndex % healthyServers.Count];
        _currentIndex = (_currentIndex + 1) % healthyServers.Count;
        var backendClient = new TcpClient(server.Host, server.Port);
        using var backendStream = backendClient.GetStream();
        await backendStream.WriteAsync(buffer, 0, length);

        var backendBuffer = new byte[1024];
        var backendLength = await backendStream.ReadAsync(backendBuffer);
        await stream.WriteAsync(backendBuffer, 0, backendLength);

        client.Close();
    }

    private async Task HealthCheckLoop()
    {
        while (true)
        {
            foreach (var server in _servers)
            {
                server.IsHealthy = await server.CheckHealthAsync();
                await _hubContext.Clients.All.SendAsync("UpdateServerStatus", server.Host, server.Port, server.IsHealthy);
            }
            await Task.Delay(_healthCheckInterval);
        }
    }
}
