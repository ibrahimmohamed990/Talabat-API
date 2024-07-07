using Store.Services.Services.User_Service.Dtos;

namespace Store.Services.Services.User_Service
{
    public interface IUserService
    {
        Task<UserDto> Register(RegisterDto input);
        Task<UserDto> Login(LoginDto input);
    }
}
