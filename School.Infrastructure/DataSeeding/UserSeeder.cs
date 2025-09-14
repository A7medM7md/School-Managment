using Microsoft.AspNetCore.Identity;
using School.Data.Entities.Identity;

namespace School.Infrastructure.DataSeeding
{
    public static class UserSeeder
    {
        public static async Task SeedAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole<int>> roleManager)
        {

            // Admin user
            var adminEmail = "admin@school.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new AppUser
                {
                    UserName = "Admin",
                    Email = adminEmail,
                    FullName = "Admin",
                    PhoneNumber = "00000000000",
                    Address = "Tanta",
                    Country = "Egypt",
                    EmailConfirmed = true, // Email, PhoneNumber Confirmations Required When SigningIn
                    PhoneNumberConfirmed = true
                };

                var createResult = await userManager.CreateAsync(adminUser, "Admin123#");
                if (!createResult.Succeeded)
                {
                    throw new Exception($"Failed to create default admin: {string.Join(" | ", createResult.Errors.Select(e => e.Description))}");
                }
            }

            // Ensure Admin role exists
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole<int>("Admin"));
            }

            // Add user to Admin role if not already
            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

        }
    }
}