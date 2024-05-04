using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TripETL.Domain.Interfaces;

namespace TripETL.Data;

public static class DataServicesConfiguration
{
    public static void ConfigureDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TripDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<ITripRepository, TripRepository>();
    }
}