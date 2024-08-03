using Microsoft.EntityFrameworkCore;
using WebApplication1.DAL;

namespace WebApplication1
{
    public static class ServiceRegistration
    {
        public static void Register(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllersWithViews();
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("Default"));
            });
        }
    }
}
