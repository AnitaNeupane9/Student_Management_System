using AnitaSMS.web.Models;
using Microsoft.AspNetCore.Identity;

namespace AnitaSMS.Data
{
    public class SeedingData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] Roles = { "ADMIN", "STUDENT" };

            foreach (string roleName in Roles)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }


            if (await _userManager.FindByNameAsync("admin@mail.com") == null)
            {
                var role = _roleManager.FindByNameAsync("ADMIN").Result;

                var user = new ApplicationUser()
                {

                    FirstName = "Admin",
                    MiddleName = "",
                    LastName = "User",
                    IsActive = true,
                    UserRoleId = role.Id,
                    UserName = "admin@mail.com",
                    Email = "admin@mail.com",
                    PhoneNumber = "9800000000",
                    Address = "Kathmandu",
                    CreatedBy = "superadmin",
                    CreatedDate = DateTime.Now
                };

                var res = await _userManager.CreateAsync(user, "@Admi1");
                await _userManager.SetLockoutEnabledAsync(user, false);
                if (res.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "ADMIN");
                }
            }
        }
    }
}
