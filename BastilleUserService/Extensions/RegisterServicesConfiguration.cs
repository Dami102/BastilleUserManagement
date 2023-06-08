using BastilleUserLibrary.Infrastructure.External_Services;
using BastilleUserLibrary.Infrastructure.Repositories;
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
            services.AddSingleton<IRefreshTokenValidator, RefreshTokenValidator>();
            services.AddSingleton<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddAutoMapper(typeof(MappingProfiles));
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ICloudinaryServices, CloudinaryServices>();
        }
    }
}
