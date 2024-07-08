using Microsoft.AspNetCore.Identity;
using Store.Data.Entities.Identity_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository
{
    public class AppIdentityContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Adminstrator",
                    Email = "admin@talabat.com",
                    UserName = "adminstrator",
                    Address = new Address
                    {
                        FirstName = "Ibrahim",
                        LastName = "Mohamed",
                        City = "Tanta",
                        State = "Gharbia",
                        Street = "001",
                        ZipCode = "123321"
                    }
                };
                var result = await userManager.CreateAsync(user, "Password123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }

        }

    }
}
