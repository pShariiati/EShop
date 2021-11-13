using EShop.Common;
using EShop.Common.Constants;
using EShop.Common.Extensions;
using EShop.DataLayer.Context;
using EShop.Entities;
using EShop.Services.Contracts;
using EShop.ViewModels.Products;
using EShop.ViewModels.ProductTags;
using Ganss.XSS;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EShop.Web.Areas.Admin.Controllers;

[Area(AreaConstants.AdminArea)]
public class ProductController : BaseController
{
    private readonly ICategoryService _categoryService;
    private readonly IProductService _productService;
    private readonly IProductImageService _productImageService;
    private readonly IProductTagService _productTagService;
    private readonly IHtmlSanitizer _htmlSanitizer;
    private readonly IUnitOfWork _uow;

    public ProductController(
        ICategoryService categoryService,
        IUnitOfWork uow,
        IProductService productService,
        IProductImageService productImageService,
        IProductTagService productTagService,
        IHtmlSanitizer htmlSanitizer)
    {
        _categoryService = categoryService;
        _uow = uow;
        _productService = productService;
        _productImageService = productImageService;
        _productTagService = productTagService;
        _htmlSanitizer = htmlSanitizer;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _productService.GetProductsPreviewAsync());
    }

    public async Task<IActionResult> Add()
    {
        var categories = await _categoryService.AllMainCategoriesAsync();
        ViewBag.MainCategories = categories.ToList().CreateSelectListItem(addChooseOneItem: false);
        return View(new AddProductViewModel());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(AddProductViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var categories = await _categoryService.AllMainCategoriesAsync();
            ViewBag.MainCategories = categories.ToList().CreateSelectListItem(addChooseOneItem: false, selectedItem: model.CategoryId);
            ModelState.AddModelError(string.Empty, PublicConstantStrings.ModelStateErrorMessage);
            return View(model);
        }

        var productTags = new List<string>();
        if (model.Tags is not null)
        {
            var convertedTags = JsonConvert.DeserializeObject<List<TagifyValueViewModel>>(model.Tags);
            productTags = convertedTags
                .Where(x => x.Value != null)
                .Select(x => x.Value.Trim())
                .Distinct()
                .ToList();
            if (productTags.Count > 10 || productTags.Any(x => x.Length > 100))
            {
                return View("Error");
            }
        }
        var product = new Product()
        {
            CategoryId = model.CategoryChildrenId,
            Description = model.Description,
            Price = model.Price,
            Title = model.Title
        };
        foreach (var property in model.Properties)
        {
            var splittedProperty = property.Split("|||", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            product.ProductProperties.Add(new ProductProperty()
            {
                Title = splittedProperty[0],
                Value = splittedProperty[1]
            });
        }

        foreach (var image in model.Images)
        {
            var imageExtension = Path.GetExtension(image.FileName);
            var imageName = Guid.NewGuid().ToString("N");
            image.SaveImage(imageName, imageExtension, "products");
            product.ProductImages.Add(new ProductImage()
            {
                Title = imageName + imageExtension
            });
        }

        var tags = _productTagService.GetTags(productTags);
        foreach (var tagTitle in productTags)
        {
            var addedTag = tags.SingleOrDefault(x => x.Title == tagTitle);

            var tagToAdd = addedTag ?? new ProductTag()
            {
                Title = tagTitle
            };

            product.ProductProductTags.Add(new ProductProductTag()
            {
                ProductTag = tagToAdd
            });
        }
        await _productService.AddAsync(product);
        await _uow.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<JsonResult> GetSubCategories(int mainCategoryId)
    {
        var subCategories = await _categoryService.GetCategoryChildrenAsync(mainCategoryId);
        return Json(subCategories);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var editModel = await _productService.GetProductToEdit(id);
        if (editModel is null)
            return View("NotFound");
        var categories = await _categoryService.AllMainCategoriesAsync();
        ViewBag.MainCategories = categories.ToList().CreateSelectListItem(addChooseOneItem: false, selectedItem: editModel.CategoryId);
        return View(editModel);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditProductViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var categories = await _categoryService.AllMainCategoriesAsync();
            ViewBag.MainCategories = categories.ToList().CreateSelectListItem(addChooseOneItem: false, selectedItem: model.CategoryId);
            ModelState.AddModelError(string.Empty, PublicConstantStrings.ModelStateErrorMessage);
            return View(model);
        }
        var productTags = new List<string>();
        if (model.SelectedTags is not null)
        {
            var convertedTags = JsonConvert.DeserializeObject<List<TagifyValueViewModel>>(model.SelectedTags);
            productTags = convertedTags
                .Where(x => x.Value != null)
                .Select(x => x.Value.Trim())
                .Distinct()
                .ToList();
            if (productTags.Count > 10 || productTags.Any(x => x.Length > 100))
            {
                return View("Error");
            }
        }
        var product = await _productService.GetProductToUpdateAsync(model.Id);
        if (product.ProductImages.Any() == false &&
            (model.Images == null || !model.Images.Any())
        )
        {
            var categories = await _categoryService.AllMainCategoriesAsync();
            ViewBag.MainCategories = categories.ToList().CreateSelectListItem(addChooseOneItem: false, selectedItem: model.CategoryId);
            ModelState.AddModelError(nameof(EditProductViewModel.Images), "لطفا حداقل یک عکس را برای محصول انتخاب کنید");
            return View(model);
        }
        product.ProductProperties.Clear();
        foreach (var property in model.Properties)
        {
            var splittedProperty = property.Split("|||", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            product.ProductProperties.Add(new ProductProperty()
            {
                Title = splittedProperty[0],
                Value = splittedProperty[1]
            });
        }

        foreach (var image in model.Images)
        {
            var imageExtension = Path.GetExtension(image.FileName);
            var imageName = Guid.NewGuid().ToString("N");
            image.SaveImage(imageName, imageExtension, "products");
            product.ProductImages.Add(new ProductImage()
            {
                Title = imageName + imageExtension
            });
        }

        product.ProductProductTags.Clear();
        var tags = _productTagService.GetTags(productTags);
        foreach (var tagTitle in productTags)
        {
            var addedTag = tags.SingleOrDefault(x => x.Title == tagTitle);

            var tagToAdd = addedTag ?? new ProductTag()
            {
                Title = tagTitle
            };

            product.ProductProductTags.Add(new ProductProductTag()
            {
                ProductTag = tagToAdd
            });
        }

        product.Price = model.Price;
        product.Title = model.Title;
        //product.Description = model.Description;
        product.Description = _htmlSanitizer.Sanitize(model.Description);
        product.CategoryId = model.CategoryChildrenId;
        _productService.Update(product);
        await _uow.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveProductPicture(string productImageName)
    {
        await _productImageService.RemoveProductImageByNameAsync(productImageName);
        WorkWithImages.RemoveImage(productImageName, "products");
        await _uow.SaveChangesAsync();
        return Json(true);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var productToDelete = _productService.GetProductToDelete(id);
        if (productToDelete is null)
            return View("Error");
        _productService.Remove(productToDelete);
        await _uow.SaveChangesAsync();
        foreach (var image in productToDelete.ProductImages)
        {
            WorkWithImages.RemoveImage(image.Title, "products");
        }
        return RedirectToAction(nameof(Index));
    }
}
