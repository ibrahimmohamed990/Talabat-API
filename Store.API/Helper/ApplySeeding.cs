using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.Data.Context;
using Store.Data.Entities.Identity_Entities;
using Store.Repository;

namespace Store.API.Helper
{
    public class ApplySeeding
    {
        public static async Task ApplySeedingAsync(WebApplication app)
        {
            using(var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                try
                {
                    var context = services.GetRequiredService<StoreDbContext>();
                    var userManager = services.GetRequiredService<UserManager<AppUser>>();

                    await context.Database.MigrateAsync(); // if any appending migration found, it will be updated to database - if database is aleardy created, it will not create a new one.
                    
                    await StoreContextSeed.SeedAsync(context, loggerFactory);
                    await AppIdentityContextSeed.SeedUserAsync(userManager);
                }
                catch (Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<StoreDbContext>();
                    logger.LogError(ex.Message);
                }
            }

        }
    }
}
