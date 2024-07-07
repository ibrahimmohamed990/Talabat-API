﻿using AdminDashboard.Helpers;
using AdminDashboard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminDashboard.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            // Get All Roles
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }
        [Authorize(Policy = "AccessDenied")]
        [HttpPost]
        public async Task<IActionResult> Create (RoleFormViewModel model)
        {
            if(ModelState.IsValid)
            {
                var roleExists = await _roleManager.RoleExistsAsync(model.Name);

                if (!roleExists)
                {
                    await _roleManager.CreateAsync(new IdentityRole(model.Name.Trim()));

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Name", "Role Is Exist");
                    return View("Index", await _roleManager.Roles.ToListAsync());
                }
            }

            return RedirectToAction(nameof(Index));
        }
        [Authorize(Policy = "AccessDenied")]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            await _roleManager.DeleteAsync(role);

            return RedirectToAction(nameof(Index));
        }
        [Authorize(Policy = "AccessDenied")]
        public async Task<IActionResult> Edit (string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            var mappedRole = new RoleViewModel()
            {
                Name = role.Name,
            };

            return View(mappedRole);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var roleExist = await _roleManager.RoleExistsAsync(model.Name);

                if(!roleExist)
                {
                    var role = await _roleManager.FindByIdAsync(model.Id);
                    role.Name = model.Name;
                    await _roleManager.UpdateAsync(role);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Name", "Role Is Exist");
                    return View("Index", await _roleManager.Roles.ToListAsync());
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}