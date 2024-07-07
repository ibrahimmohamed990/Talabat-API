using AdminDashboard.Helpers;
using AdminDashboard.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Data.Entities.Identity_Entities;


namespace AdminDashboard.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IMapper _mapper;

		public UserController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
			_mapper = mapper;
		}
        public async Task<IActionResult> Index()
        {
			var users = await _userManager.Users.ToListAsync(); 
			return View(users);
		}
        [Authorize(Policy = "AccessDenied")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View(new RegisterVM());
		}
        [HttpPost]
        public async Task<IActionResult> Create(RegisterVM input)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(input.Email);
                if (user is not null)
                    throw new Exception();

                var appUser = new AppUser
                {
                    DisplayName = input.DisplayName,
                    Email = input.Email,
                    UserName = input.DisplayName
                };
                var result = await _userManager.CreateAsync(appUser, input.Password);

                if (!result.Succeeded)
                    throw new Exception(result.Errors.Select(x => x.Description).FirstOrDefault());

                return RedirectToAction(nameof(Index));
            }
            return View(input);
	    }
        [Authorize(Policy = "AccessDenied")]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var allRoles = await _roleManager.Roles.ToListAsync();

            var viewModel = new UserRoleViewModel()
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = allRoles.Select(r => new RoleViewModel()
                {
                    Id = r.Id,
                    Name = r.Name,
                    IsSelected = _userManager.IsInRoleAsync(user, r.Name).Result
                }).ToList()
            };
            return View(viewModel); 
        }
        [Authorize(Policy = "AccessDenied")]
        [HttpPost]
        public async Task<IActionResult> Edit(string id, UserRoleViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in model.Roles)
            {
                if (userRoles.Any(r => r == role.Name) && !role.IsSelected)
                    await _userManager.RemoveFromRoleAsync(user, role.Name);

                if (!userRoles.Any(r => r == role.Name) && role.IsSelected)
                    await _userManager.AddToRoleAsync(user, role.Name);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
