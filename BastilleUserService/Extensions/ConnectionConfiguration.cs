﻿using BastilleIUserLibrary.Domain.Model;
using BastilleUserLibrary.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BastilleUserService.Extensions
{
    public static class ConnectionConfiguration
    {
        public static void AddContextAndConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnsectionString")));
            services.AddCors(o =>
            {
                o.AddPolicy("AllowAll", builder =>
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    );
            });
            services.AddIdentity<User, IdentityRole>(x =>
            {
                x.Password.RequiredLength = 5;
                x.SignIn.RequireConfirmedEmail = false;
                x.User.RequireUniqueEmail = true;
               /* x.SignIn.RequireConfirmedEmail = true;*/
                x.Password.RequireNonAlphanumeric = false;
                x.Password.RequireUppercase= false;
                x.Password.RequireLowercase= false;
                x.Password.RequireDigit= false;

            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
        }
    }
}
