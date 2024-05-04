using Microsoft.Extensions.DependencyInjection;
using TripETL.Domain.Interfaces;

namespace TripETL.App;

public static class ServiceConfiguration
{
    public static void ConfigureServices(IServiceCollection services)
    { 
        services.AddScoped<ITripService, TripService>();
    }
}