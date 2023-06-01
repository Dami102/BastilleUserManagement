using BastilleUserLibrary.Infrastructure.External_Services;
using BastilleUserService.Core.Interfaces;
using BastilleUserService.Core.Services;
using BastilleUserService.Core.Utilities.Profiles;

namespace BastilleUserService.Extensions
{
    public static class RegisteredServicesConfiguration
    {
        public static void AddRegisteredServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserProfileService, UserProfileService>();
            services.AddSingleton<IEmailService, EmailService>();
            services.AddAutoMapper(typeof(MappingProfiles));
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ICloudinaryServices, CloudinaryServices>();
        }
    }
}
