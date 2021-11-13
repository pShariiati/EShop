using EShop.Common.Constants;
using EShop.DataLayer.Context;
using EShop.Entities.Identity;
using EShop.Services.Contracts.Identity;
using EShop.ViewModels.Roles;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Web.Areas.Admin.Controllers;

[Area(AreaConstants.AdminArea)]
public class RoleController : BaseController
{
    private readonly IRoleManagerService _roleManagerService;
    private readonly IUnitOfWork _uow;

    public RoleController(IRoleManagerService roleManagerService, IUnitOfWork uow)
    {
        _roleManagerService = roleManagerService;
        _uow = uow;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _roleManagerService.GetRolesPreviewAsync());
    }

    public IActionResult Add()
    {
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(AddRoleViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _roleManagerService.CreateAsync(new Role(model.Name));
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        else
            ModelState.AddModelError(string.Empty, PublicConstantStrings.ModelStateErrorMessage);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CheckRoleNameForAdd(string name)
    {
        var isRoleExists = await _roleManagerService.RoleExistsAsync(name);
        return Json(!isRoleExists);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CheckRoleNameForEdit(string name, int id)
    {
        var isRoleExists = await _roleManagerService.IsRoleExistsForEdit(id, name);
        return Json(!isRoleExists);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var model = await _roleManagerService.GetForEditAsync(id);
        if (model is null)
            return View("NotFound");
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditRoleViewModel model)
    {
        if (ModelState.IsValid)
        {
            var role = await _roleManagerService.FindByIdAsync(model.Id.ToString());
            if (role is null)
                return View("Error");
            role.Name = model.Name;
            var result = await _roleManagerService.UpdateAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        else
            ModelState.AddModelError(string.Empty, PublicConstantStrings.ModelStateErrorMessage);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int roleId)
    {
        var role = await _roleManagerService.RoleToDelete(roleId);
        if (role is null)
            return View("NotFound");
        var result = await _roleManagerService.DeleteAsync(role);
        if (!result.Succeeded)
            return View("Error");
        return RedirectToAction(nameof(Index));
    }
}
