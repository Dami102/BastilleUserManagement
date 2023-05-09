using BastilleIUserLibrary.Domain.Model;
using BastilleUserLibrary.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace BastilleUserService.Extensions
{
    public static class ConnectionConfiguration
    {
        public static void AddContextAndConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnsectionString")));

            services.AddIdentity<User, IdentityRole>(x =>
            {
                x.Password.RequiredLength = 10;
                x.SignIn.RequireConfirmedEmail = true;
                x.User.RequireUniqueEmail = true;
                x.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
        }
    }
}
