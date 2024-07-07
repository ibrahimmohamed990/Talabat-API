using Store.Data.Entities.Identity_Entities;

namespace Store.Services.Services.Token_Service
{
    public interface ITokenService
    {
        string GenerateToken(AppUser appUser);

    }
}
