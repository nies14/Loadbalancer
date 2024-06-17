using BackendServer;

public class Program
{
    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
                webBuilder.ConfigureAppConfiguration((context, config) =>
                {
                    if (args.Length > 0)
                    {
                        config.AddInMemoryCollection(new Dictionary<string, string>
                        {
                            { "ServerName", args[2] }
                        }!);
                    }
                });
            });
}