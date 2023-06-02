using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OptiOverflow.Core.Constants;
using OptiOverflow.Core.Entities;
using OptiOverflow.Repository.DatabaseContext;

namespace OptiOverflow.Repository.Seeder;

public class DataSeeder
{

    public async Task SeedRoles(RoleManager<IdentityRole<Guid>> roleManager)
    {
        if (!roleManager.Roles.Any())
        {
            var roles = new List<IdentityRole<Guid>>
            {
                new() { Name = UserRoles.Admin, NormalizedName = UserRoles.Admin.ToUpper() },
                new() { Name = UserRoles.User, NormalizedName = UserRoles.User.ToUpper() }
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role.Name))
                    await roleManager.CreateAsync(role);
            }
        }
    }

    public async Task SeedAdmin(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        var user = new ApplicationUser
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
        };
        var applicationUser = await userManager.FindByEmailAsync(user.Email);
        if (applicationUser == null)
        {
            var response = await userManager.CreateAsync(user, "123@456");
            if (response.Succeeded)
            {
                var roleAssignResult = await userManager.AddToRoleAsync(user, UserRoles.Admin);
                if (!roleAssignResult.Succeeded)
                    await userManager.DeleteAsync(user);
                if (context.UserProfile != null && !await context.UserProfile.AnyAsync(x => x.AccountId == user.Id))
                    await context.AddAsync(new UserProfile
                        { AccountId = user.Id, FirstName = "Opti", LastName = "Admin", Phone = "+880xxxxxxxxxx" });
            }
            await context.SaveChangesAsync();
        }
    }
}