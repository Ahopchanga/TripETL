using Microsoft.Extensions.DependencyInjection;
using TripETL.Data;
using TripETL.Domain.Interfaces;

namespace TripETL
{
    public static class ServiceConfiguration
    {
        public static void ConfigureServices(IServiceCollection services)
        { 
            services.AddScoped<ITripService, TripService>();
        }
    }
}