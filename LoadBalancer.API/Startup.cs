using LoadBalancer.Core.Services;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace LoadBalancer.API;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddRazorPages();
        services.AddSignalR();
    }

    public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseDefaultFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
            endpoints.MapHub<ServerStatusHub>("/serverStatusHub");
        });

        //var url = $"http://localhost:5065/api/Server/list";
        //using var client = new HttpClient();
        //var response = await client.GetAsync(url);
        //var servers = new List<IBackendServerCommunicator>();

        //if (response.IsSuccessStatusCode)
        //{
        //    var jsonString = await response.Content.ReadAsStringAsync();
        //    var servers1 = JsonConvert.DeserializeObject<List<ServerResponse>>(jsonString);

        //    foreach(var server in servers1)
        //    {
        //        servers.Add(new BackendServerCommunicator(server.Host, server.Port));
        //    }

        //}

        // Initialize the Load Balancer
        var servers = new List<IBackendServerCommunicator>
            {
                new BackendServerCommunicator("localhost", 8080),
                new BackendServerCommunicator("localhost", 8081)
            };

        var loadBalancer = new LoadBalancer.Core.Services.LoadBalancer(
            servers,
            TimeSpan.FromSeconds(10),
            app.ApplicationServices.GetRequiredService<IHubContext<ServerStatusHub>>()
        );

        _ = Task.Run(() => loadBalancer.StartAsync());
    }

}
