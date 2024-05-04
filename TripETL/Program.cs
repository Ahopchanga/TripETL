using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TripETL;

class Program
{
    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        var startup = host.Services.GetService<Startup>();
        startup.Configure();

        host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                var startup = new Startup();
                startup.ConfigureServices(services);
                services.AddSingleton(startup);
            });
    }
}