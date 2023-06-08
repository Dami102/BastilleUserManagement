using BastilleIUserLibrary.Domain.Enums;
using BastilleIUserLibrary.Domain.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BastilleUserLibrary.Infrastructure.Extensions
{
    public class SeedServiceExtension
    {
        //public static async Task Seed(IApplicationBuilder builder)
        //{
        //    using var servisescope = builder.ApplicationServices.CreateScope();
        //    var context = servisescope.ServiceProvider.GetService<ApplicationDbContext>();
        //    /*ar currDir = Directory.GetCurrentDirectory();
        //    var baseDir = Directory.GetParent(currDir);*/
        //    string filePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, @"BastilleUserLibrary.Infrastructure\Data\");
        //    if (await context.Database.EnsureCreatedAsync()) return;
        //    /*  await _dbContext.Database.EnsureCreatedAsync();*/
        //    if (!context.Roles.Any())
        //    {
        //        /*var s = _dbContext.GetService<RoleManager<IdentityRole>>();*/
        //        var rolemanager = servisescope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        //        var text = await File.ReadAllTextAsync(filePath + "Role.json");
        //        var roles = JsonSerializer.Deserialize<List<IdentityRole>>(text);
        //        foreach (var role in roles)
        //        {
        //            await rolemanager.CreateAsync(role);
        //        }
        //    }

        //    if (!context.Users.Any())
        //    {
        //        var usermanager = servisescope.ServiceProvider.GetRequiredService<UserManager<User>>();
        //        var text = await File.ReadAllTextAsync(filePath + "Users.json");
        //        var users = JsonSerializer.Deserialize<List<User>>(text);
        //        foreach (var user in users)
        //        {
        //            await usermanager.CreateAsync(user, "password12345");
        //            await usermanager.AddToRoleAsync(user, UserRole.Admin.ToString());
        //            await context.Users.AddAsync(user);

        //        }


        //        await context.SaveChangesAsync();
        //    }
        //}


        public static async Task IntializeAsync(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {

                var services = scope.ServiceProvider;
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = services.GetRequiredService<UserManager<User>>();
                var context = services.GetRequiredService<ApplicationDbContext>();



                string[] userRoles = { "Admin", "Seller", "Buyer" };

                IdentityResult roleResult;

                foreach (var userRole in userRoles)
                {
                    var roleExist = await roleManager.RoleExistsAsync(userRole);

                    if (!roleExist)
                    {
                        roleResult = await roleManager.CreateAsync(new IdentityRole(userRole));
                    }
                }

                var AdminEmail = "Admin@gmail.com";
                var password = "password";

                if (userManager.FindByEmailAsync(AdminEmail).Result == null)
                {
                    User adminUser = new()
                    {
                        Email = AdminEmail,
                        UserName = AdminEmail,
                        FirstName = "Damilola",
                        LastName = "Ebitigha",
                        PhoneNumber = "09168965528",
                        Address = "Lagos Nigeria",
                        PublicId = ""
                    };

                    IdentityResult result = userManager.CreateAsync(adminUser, password).Result;

                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(adminUser, "Admin").Wait();
                    }
                }

                await context.SaveChangesAsync();
            }
           

        }
    }
}
