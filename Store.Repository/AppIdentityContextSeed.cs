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
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Ibrahim Mohamed",
                    Email = "ibrahim@gmail.com",
                    UserName = "ibrahimmohamed",
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
                await userManager.CreateAsync(user, "Password123!");
            }

        }

    }
}
