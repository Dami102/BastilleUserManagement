using BastilleIUserLibrary.Domain.Enums;

namespace BastilleUserService.Extensions
{
    public static class AuthorizationConfiguration
    {
        public static void AddAuthorizationExtension(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminOnly", policy => policy.RequireRole(UserRole.Admin.ToString()));
                options.AddPolicy("RequireBuyerOnly", policy => policy.RequireRole(UserRole.Buyer.ToString()));
                options.AddPolicy("RequireSellerOnly", policy => policy.RequireRole(UserRole.Seller.ToString()));
                
            });
        }
    }
}
