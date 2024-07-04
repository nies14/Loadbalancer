namespace LoadBalancer.Core.Services;

public class BackendServerCommunicator : IBackendServerCommunicator
{
    public string Host { get; }
    public int Port { get; }
    public bool IsHealthy { get; set; }

    public BackendServerCommunicator(string host, int port)
    {
        Host = host;
        Port = port;
        IsHealthy = true;
    }

    public async Task<bool> CheckHealthAsync()
    {
        try
        {
            var url = $"http://{Host}:{Port}/health";
            using var client = new HttpClient();
            var response = await client.GetAsync(url);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
