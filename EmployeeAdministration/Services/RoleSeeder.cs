using Entities.Models;
using Microsoft.AspNetCore.Identity;

public static class UserSeeder
{
    public static async System.Threading.Tasks.Task SeedAdminUser(UserManager<User> userManager)
    {
        var adminEmail = "admin@example.com";
        var adminPassword = "Admin@123";

        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var adminUser = new User
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true 
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
