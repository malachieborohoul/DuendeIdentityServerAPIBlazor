using System.Security.Claims;
using API.Entities;
using API.Extensions;
using IdentityModel;
using Microsoft.AspNetCore.Identity;

namespace API.IDbInitializer;

public class DbInitializer( UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<DbInitializer> logger):IDbInitializer
{
    public void Initialize()
    {
        logger.LogInformation("Add Admin user in DB");

        if (roleManager.FindByNameAsync("Admin").Result == null)
        {
            logger.LogInformation("Admin role dosent exist create an admin role");

            roleManager.CreateAsync(new IdentityRole(Constants.Admin)).GetAwaiter().GetResult();
            roleManager.CreateAsync(new IdentityRole(Constants.User)).GetAwaiter().GetResult();

        }
        else
        {
            logger.LogInformation("Admin role exist already");

            return;
        }

        ApplicationUser adminUser = new ApplicationUser()
        {
            UserName = "admin@gmail.com",
            Email = "admin@gmail.com",
            Name = "Admin"
        };
        logger.LogInformation("Create admin user with role");

        userManager.CreateAsync(adminUser, "Admin@123").GetAwaiter().GetResult();
        userManager.AddToRoleAsync(adminUser, Constants.Admin).GetAwaiter().GetResult();

        
        logger.LogInformation("Add claims");

        var claims1 = userManager.AddClaimsAsync(adminUser, new Claim[]
        {
            new Claim(JwtClaimTypes.Name, adminUser.Name),
            new Claim(JwtClaimTypes.Role, Constants.Admin),
        }).Result;
        
        
        
        ApplicationUser user = new ()
        {
            UserName = "user@gmail.com",
            Email = "user@gmail.com",
            Name = "User"
        };
        logger.LogInformation("Create  user with role");

        userManager.CreateAsync(user, "User@123").GetAwaiter().GetResult();
        userManager.AddToRoleAsync(user, Constants.User).GetAwaiter().GetResult();

        
        logger.LogInformation("Add claims");

        var claims2 = userManager.AddClaimsAsync(user, new Claim[]
        {
            new Claim(JwtClaimTypes.Name, user.Name),
            new Claim(JwtClaimTypes.Role, Constants.User),
        }).Result;

    }
}