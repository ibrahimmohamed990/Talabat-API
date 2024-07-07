using Microsoft.AspNetCore.Identity;
using Store.Data.Entities.Identity_Entities;
using Store.Services.Services.Token_Service;
using Store.Services.Services.User_Service.Dtos;

namespace Store.Services.Services.User_Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ITokenService tokenService;

        public UserService(
            UserManager<AppUser> _userManager, 
            SignInManager<AppUser> _signInManager,
            ITokenService _tokenService)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            tokenService = _tokenService;
        }
        public async Task<UserDto> Login(LoginDto input)
        {
            var user = await userManager.FindByEmailAsync(input.Email);
            if (user is null)
                throw new Exception("User Not Found");
            var result = await signInManager.CheckPasswordSignInAsync(user, input.Password, false);
            if(!result.Succeeded) 
                throw new Exception($"Failed to login {input.Email}");
            return new UserDto
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = tokenService.GenerateToken(user),
            };
        }

        public async Task<UserDto> Register(RegisterDto input)
        {
            var user = await userManager.FindByEmailAsync(input.Email);
            if (user is not null)
                return null;
            var appUser = new AppUser
            {
                DisplayName = input.DisplayName,
                Email = input.Email,
                UserName = input.DisplayName
            };
            var result = await userManager.CreateAsync(appUser, input.Password);
            
            if (!result.Succeeded)
                throw new Exception(result.Errors.Select(x => x.Description).FirstOrDefault());
            
            return new UserDto
            {
                Email = input.Email,
                DisplayName = input.DisplayName,
                Token = tokenService.GenerateToken(appUser),
            };
        }
    }
}
