using ErkinStudy.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace ErkinStudy.Infrastructure.Context
{
    public static class AppDbInitializer
    {
        public static void SeedUsers(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            if (roleManager.FindByNameAsync("Admin").Result == null)
            {
                var role = new ApplicationRole {Name = "Admin", NormalizedName = "Admin".ToUpper()};
                roleManager.CreateAsync(role).Wait();
            }
            if (userManager.FindByNameAsync("admin").Result == null)
            {
                var user = new ApplicationUser()
                {
                    UserName = "admin",
                    Email = "admin@erkinstudy.kz",
                    PhoneNumber = "87078897741"
                };
                var result = userManager.CreateAsync(user, "Qazaq123@").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
        }
    }
}
