using Microsoft.AspNetCore.Identity;

namespace Store.Data.Entities.Identity_Entities
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public Address Address { get; set; }


    }
}
