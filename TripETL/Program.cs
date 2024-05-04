using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TripETL.Data;

namespace TripETL;

class Program
{
    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;

        using var context = services.GetService<TripDbContext>();
        var pendingMigrations = context.Database.GetPendingMigrations();
        if (pendingMigrations.Any())
        {
            context.Database.Migrate();
            Console.WriteLine("Applied Migrations.");
        }
        else
        {
            Console.WriteLine("No Pending Migrations.");
        }

        host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                var startup = new Startup();
                startup.ConfigureServices(services);
            });
}