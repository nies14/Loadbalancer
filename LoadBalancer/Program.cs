using LoadBalancer.Core.Services;

var servers = new List<IBackendServer>
{
    new BackendServer("localhost", 8080),
    new BackendServer("localhost", 8081)
};

var loadBalancer = new LoadBalancer.Core.Services.LoadBalancer(servers, TimeSpan.FromSeconds(10));
await loadBalancer.StartAsync();