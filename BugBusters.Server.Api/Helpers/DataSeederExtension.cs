using BugBusters.Server.Core.Entities;
using BugBusters.Server.Repository.DatabaseContext;
using BugBusters.Server.Repository.Seeder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BugBusters.Server.Api.Helpers;

public static class DataSeederExtension
{
    public static void SeedData(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetService<ApplicationDbContext>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            context?.Database.Migrate();
            var dataSeeder = new DataSeeder();

            dataSeeder.SeedRoles(roleManager).Wait();
            dataSeeder.SeedAdmin(context, userManager).Wait();
        }
    }
}