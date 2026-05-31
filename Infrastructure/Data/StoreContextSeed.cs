using Core.Entities;
using Microsoft.AspNetCore.Identity;
using System.Reflection;

namespace Infrastructure.Data;

public class StoreContextSeed
{
    public static async Task SeedAsync(StoreContext context, UserManager<AppUser> userManager)
    {
        if (!userManager.Users.Any(x => x.UserName == "admin@email.com"))
        {
            var user = new AppUser
            {
                UserName = "admin@email.com",
                Email = "admin@email.com"
            };

            await userManager.CreateAsync(user, "Admin@123");
            await userManager.AddToRoleAsync(user, "Admin");
        }

        var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);  
    }
}