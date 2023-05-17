using BastilleUserService.Core.Interfaces;
using BastilleUserService.Core.Services;
using BastilleUserService.Core.Utilities.Profiles;

namespace BastilleUserService.Extensions
{
    public static class RegisterServicesConfiguration
    {
        public static void AddRegisteredServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfiles));
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
           
        }
    }
}
