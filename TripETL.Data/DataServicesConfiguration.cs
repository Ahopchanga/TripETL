using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TripETL.Data;

public static class DataServicesConfiguration
{
    public static void ConfigureDataServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<TripDbContext>(options =>
            options.UseSqlServer(
                config.GetConnectionString("DefaultConnection"),
                x => x.MigrationsAssembly("TripETL.Data")
            )
        );
    }
}