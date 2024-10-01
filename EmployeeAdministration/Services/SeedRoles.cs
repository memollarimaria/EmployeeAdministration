using Entities.Models;
using Microsoft.AspNetCore.Identity;

public static class RoleSeeder
{
    public static async System.Threading.Tasks.Task SeedRoles(RoleManager<UserRole> roleManager)
    {
        string[] roleNames = { "Admin", "User" };

        foreach (var roleName in roleNames)
        {
            if (await roleManager.RoleExistsAsync(roleName) == false)
            {
                var role = new UserRole { Name = roleName };
                await roleManager.CreateAsync(role);
            }
        }
    }
}
