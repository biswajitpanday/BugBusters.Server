using System.Reflection;
using BugBusters.Server.Core.Constants;
using BugBusters.Server.Core.Entities;
using Microsoft.AspNetCore.Identity;
using OptiOverflow.Repository.DatabaseContext;

namespace OptiOverflow.Repository.Seeder;

public class DataSeeder
{

    public async Task SeedRoles(RoleManager<IdentityRole<Guid>> roleManager)
    {
        if (!roleManager.Roles.Any())
        {
            var userRoles = typeof(UserRoles).GetFields(BindingFlags.Static | BindingFlags.Public)
                .Where(x => x.IsLiteral && !x.IsInitOnly)
                .Select(x => x.GetValue(null)).Cast<string>().ToList();

            foreach (var userRole in userRoles)
                if (!await roleManager.RoleExistsAsync(userRole))
                    await roleManager.CreateAsync(new IdentityRole<Guid>(userRole));
        }
    }

    public async Task SeedAdmin(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        var adminUser = new ApplicationUser
        {
            Email = "optiAdmin@optioverflow.com",
            UserName = "optiAdmin",
            PhoneNumber = "+880xxxxxxxxxx",
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow,
            LastUpdate = DateTime.UtcNow,
            EmailConfirmed = true,
            NormalizedEmail = "OPTIADMIN@OPTIOVERFLOW.COM",
            NormalizedUserName = "OPTIADMIN",
            PhoneNumberConfirmed = true,
            FirstName = "Opti",
            LastName = "Admin"
        };
        var dbUser = await userManager.FindByEmailAsync(adminUser.Email);
        if (dbUser == null)
        {
            var userCreateResponse = await userManager.CreateAsync(adminUser, "123@456");
            if (userCreateResponse.Succeeded)
            {
                var roleAssignResponse = await userManager.AddToRoleAsync(adminUser, UserRoles.Admin);
                if (!roleAssignResponse.Succeeded)
                    await userManager.DeleteAsync(adminUser);
            }
            await context.SaveChangesAsync();
        }
    }
}