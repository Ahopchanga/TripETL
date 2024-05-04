using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TripETL.Data;
using Microsoft.Extensions.Configuration;

namespace TripETL
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TripDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
        }
    }
}