namespace BackendServer;

public class Startup
{
    private readonly string _serverName;

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
        _serverName = configuration["ServerName"] ?? "Unknown";
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/", async context =>
            {
                Console.WriteLine($"Request received by Backend Server {_serverName}");
                await context.Response.WriteAsync($"Hello From Backend Server {_serverName}");
            });

            endpoints.MapGet("/health", async context =>
            {
                await context.Response.WriteAsync("Healthy");
            });
        });
    }
}
