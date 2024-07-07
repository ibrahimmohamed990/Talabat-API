using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.Data.Entities.Identity_Entities;
using Store.Services.HandleResponses;
using Store.Services.Services.User_Service;
using Store.Services.Services.User_Service.Dtos;
using System.Security.Claims;

namespace Store.API.Controllers
{

    public class AccountController : BaseController
    {
        private readonly IUserService userService;
        private readonly UserManager<AppUser> userManager;

        public AccountController(IUserService _userService, UserManager<AppUser> _userManager)
        {
            userService = _userService;
            userManager = _userManager;
        }
        [HttpPost]
        public async Task<ActionResult<UserDto>> Login([FromQuery]LoginDto input)
        {
            var user = await userService.Login(input);
            if (user is null)
                return Unauthorized(new CustomException(401));
            return Ok(user);
        }
        [HttpPost]
        public async Task<ActionResult<UserDto>> Register([FromQuery]RegisterDto input)
        {
            var user = await userService.Register(input);
            if (user is null)
                return BadRequest(new CustomException(400, "Email Aleardy Exists!"));
            
            return Ok(user);
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUserDetails()
        {
            var email = User?.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(email);
            return Ok(user);
        }
    }
}
