using Microsoft.Extensions.Configuration;
using TripETL.Data;
using Microsoft.Extensions.DependencyInjection;

namespace TripETL
{
    public class Startup
    {
        public IConfiguration Configuration { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            Configuration = FetchConfiguration();
            services.ConfigureDataServices(Configuration);
        }

        private IConfiguration FetchConfiguration() 
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }
    }
}