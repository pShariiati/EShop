using EShop.Common.Constants;
using EShop.Entities.Identity;
using EShop.Services.Contracts.Identity;
using EShop.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Web.Areas.Admin.Controllers
{
    [Area(AreaConstants.AdminArea)]
    public class UserController : BaseController
    {
        private readonly IUserManagerService _userManagerService;
        private readonly IRoleManagerService _roleManagerService;

        public UserController(IUserManagerService userManagerService, IRoleManagerService roleManagerService)
        {
            _userManagerService = userManagerService;
            _roleManagerService = roleManagerService;
        }

        public IActionResult Index(int page = 1)
        {
            var users = _userManagerService.UsersWithPaginationPreview(page, 2);
            return View(users);
        }

        public async Task<IActionResult> Add()
        {
            ViewBag.Roles = await _roleManagerService.GetRoleNamesAsync();
            return View(new AddUserViewModel());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!await _roleManagerService.CheckRolesAsync(model.SelectedRoles))
                    return View("Error");
                var user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserName = model.UserName,
                    EmailConfirmed = true,
                    CreatedDateTime = DateTime.Now,
                    IsActive = true
                };
                var result = await _userManagerService.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManagerService.AddToRolesAsync(user, model.SelectedRoles);
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            else
            {
                ModelState.AddModelError(string.Empty, PublicConstantStrings.ModelStateErrorMessage);
            }
            ViewBag.Roles = await _roleManagerService.GetRoleNamesAsync();
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var model = await _userManagerService.GetUserForEditAsync(id);
            if (model is null)
                return View("NotFound");
            ViewBag.Roles = await _roleManagerService.GetRoleNamesAsync();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!await _roleManagerService.CheckRolesAsync(model.SelectedRoles))
                    return View("Error");
                var user = await _userManagerService.FindByIdAsync(model.Id.ToString());
                if (user is null)
                    return View("NotFound");
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.UserName = model.UserName;
                if (!string.IsNullOrWhiteSpace(model.Password))
                {
                    user.PasswordHash = _userManagerService.PasswordHasher.HashPassword(user, model.Password);
                }
                var result = await _userManagerService.UpdateAsync(user);
                if (result.Succeeded)
                {
                    var roles = await _userManagerService.GetRolesAsync(user);
                    await _userManagerService.RemoveFromRolesAsync(user, roles.ToArray());

                    await _userManagerService.AddToRolesAsync(user, model.SelectedRoles);
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            else
            {
                ModelState.AddModelError(string.Empty, PublicConstantStrings.ModelStateErrorMessage);
            }
            ViewBag.Roles = await _roleManagerService.GetRoleNamesAsync();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUserStatus(string userId)
        {
            var user = await _userManagerService.FindByIdAsync(userId);
            if (user is null)
                return View("NotFound");
            user.IsActive = !user.IsActive;
            var result = await _userManagerService.UpdateAsync(user);
            await _userManagerService.UpdateSecurityStampAsync(user);
            if (!result.Succeeded)
                return View("Error");
            return RedirectToAction(nameof(Index));
        }
    }
}
