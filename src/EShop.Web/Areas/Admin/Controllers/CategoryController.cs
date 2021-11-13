using EShop.Common;
using EShop.Common.Constants;
using EShop.DataLayer.Context;
using EShop.Entities;
using EShop.Services.Contracts;
using EShop.ViewModels.Categories;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Web.Areas.Admin.Controllers;

[Area(AreaConstants.AdminArea)]
public class CategoryController : BaseController
{
    private readonly ICategoryService _categoryService;
    private readonly IUnitOfWork _uow;

    public CategoryController(ICategoryService categoryService, IUnitOfWork uow)
    {
        _categoryService = categoryService;
        _uow = uow;
    }

    public async Task<IActionResult> Index()
    {
        var categories = await _categoryService.AllMainCategoriesAsync();
        return View(categories);
    }

    public async Task<IActionResult> Add()
    {
        var categories = await _categoryService.AllMainCategoriesAsync();
        ViewBag.MainCategories = categories.ToList().CreateSelectListItem(firstItemText: "خودش سر دسته باشد");
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(AddCategoryViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var categories = await _categoryService.AllMainCategoriesAsync();
            ViewBag.MainCategories = categories.ToList()
                .CreateSelectListItem(model.ParentId, firstItemText: "خودش سر دسته باشد");
            ModelState.AddModelError(string.Empty, PublicConstantStrings.ModelStateErrorMessage);
            return View(model);
        }
        await _categoryService.AddAsync(new Category()
        {
            Title = model.Title,
            ParentId = model.ParentId == 0 ? null : model.ParentId
        });
        await _uow.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var categoryToDelete = _categoryService.GetToDelete(id);
        if (categoryToDelete != null)
        {
            _categoryService.Remove(id);
            await _uow.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var categories = await _categoryService.AllMainCategoriesAsync(id);
        var category = await _categoryService.FindByIdAsync(id);
        ViewBag.MainCategories = categories.ToList()
            .CreateSelectListItem(category.ParentId, firstItemText: "خودش سر دسته باشد");
        var editCatViewModel = new EditCategoryViewModel()
        {
            Id = category.Id,
            ParentId = category.ParentId,
            Title = category.Title
        };
        return View(editCatViewModel);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditCategoryViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var categories = await _categoryService.AllMainCategoriesAsync(model.Id);
            ViewBag.MainCategories = categories.ToList()
                .CreateSelectListItem(model.ParentId, firstItemText: "خودش سر دسته باشد");
            ModelState.AddModelError(string.Empty, PublicConstantStrings.ModelStateErrorMessage);
            return View(model);
        }

        if (model.Id == model.ParentId)
            return View("Error");
        _categoryService.Update(new Category
        {
            Id = model.Id,
            ParentId = model.ParentId == 0 ? null : model.ParentId,
            Title = model.Title
        });
        await _uow.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> ShowCategoryChildren(int mainCatId)
    {
        var categories = await _categoryService.GetCategoryChildrenAsync(mainCatId);
        return View("_ShowCategoryeChildrenPartial", categories);
    }
}
