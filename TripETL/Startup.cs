using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TripETL.Data;
using TripETL.Domain.Interfaces;

namespace TripETL;

public class Startup
{
    private IServiceCollection _services;
    public IConfiguration Configuration { get; private set; }

    public void ConfigureServices(IServiceCollection services)
    {
        Configuration = FetchConfiguration();
        services.ConfigureDataServices(Configuration);
        ServiceConfiguration.ConfigureServices(services);
        
        _services = services;
    }

    public void Configure()
    {
        using var scope = _services.BuildServiceProvider().CreateScope();
        var serviceProvider = scope.ServiceProvider;
        var context = serviceProvider.GetRequiredService<TripDbContext>();
        var tripService = serviceProvider.GetRequiredService<ITripService>();
        
        var pendingMigrations = context.Database.GetPendingMigrations();
        if (pendingMigrations.Any())
        {
            context.Database.Migrate();
            Console.WriteLine("Migrations Applied.");
        }
        else
        {
            Console.WriteLine("No Pending Migrations.");
        }
                
        if (!CheckIfDataShouldBeInitialized(context)) return;
        var trips = tripService.ReadCsvAsync("./sample-cab-data.csv")
            .GetAwaiter()
            .GetResult();
        tripService.LoadDataAsync(trips)
            .GetAwaiter()
            .GetResult();
        tripService.RemoveWhitespaceInStringFieldsAsync()
            .GetAwaiter()
            .GetResult();
    }

    private IConfiguration FetchConfiguration() 
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(@"C:\Users\ahopc\RiderProjects\TripETL\TripETL\bin\Debug\net7.0\appsettings.json", optional: false, reloadOnChange: true)            .Build();
    }

    private bool CheckIfDataShouldBeInitialized(TripDbContext context)
    {
        return !context.Trips.Any();
    }
}