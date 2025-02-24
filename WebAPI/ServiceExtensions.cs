using Infrastructure.Persistance.Data;
using Microsoft.EntityFrameworkCore;

namespace WebAPI
{
    public static class ServiceExtensions
    {
        public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
                options.EnableSensitiveDataLogging(true);  
            });
        }
    }
}
