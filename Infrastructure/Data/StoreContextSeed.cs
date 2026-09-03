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

        if (!context.system_settings.Any())
        {
            context.system_settings.Add(new SystemSetting
            {
                project_name = "ระบบบริหารพัสดุ",
                logo_path = "/institute-Logo.png",
                is_active = true,
                created_at = DateTime.UtcNow
            });
            await context.SaveChangesAsync();
        }

        var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);  
    }
}
