using Microsoft.AspNetCore.Identity;

namespace School.Infrastructure.DataSeeding
{
    public static class RoleSeeder
    {
        public static async Task SeedAsync(RoleManager<IdentityRole<int>> roleManager)
        {
            string[] roles = { "Admin", "User" };

            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole<int>(roleName));
                }
            }
        }
    }
}
