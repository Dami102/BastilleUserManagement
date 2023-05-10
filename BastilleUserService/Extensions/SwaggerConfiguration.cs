using Microsoft.OpenApi.Models;
using System.Reflection;

namespace BastilleUserService.Extensions
{
    public static class SwaggerConfiguration
    {
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc("BastilleOpenAPI", new()
                {
                    Title = "Bastille API",
                    Version = "1",
                    Description = "Ask Dami, only he knows"
                });
                // Set the comments path for the Swagger JSON and UI.
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //setupAction.IncludeXmlComments(xmlPath);

                // To Enable authorization using Swagger (JWT) 
                setupAction.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                });
                setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
               
            });
        }
    }
}
