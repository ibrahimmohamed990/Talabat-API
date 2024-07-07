using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Store.Data.Entities.Identity_Entities;

namespace Store.Data.Context
{
    public class StoreIdentityDBContext : IdentityDbContext<AppUser>
    {
        public StoreIdentityDBContext(DbContextOptions options) : base(options)
        {

        }
    }
}
