using EShop.Services.Contracts;
using EShop.ViewModels.Products;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Web.Controllers;

public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;

    public ProductController(IProductService productService, ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index(
            List<int> selectedCategories,
            int selectedMinPrice,
            int selectedMaxPrice,
            int page = 1,
            string s = "",
            ProductSearchConditionEnum condition = ProductSearchConditionEnum.Newest
    )
    {
        var productsWithPagination = _productService.GetProductsWithFilterAndPagination(
            selectedCategories, selectedMinPrice, selectedMaxPrice, page, s);
        var model = new SearchingProductsViewModel
        {
            Products = productsWithPagination.Products,
            CurrentPage = productsWithPagination.CurrentPage,
            PagesCount = productsWithPagination.PagesCount,
            Condition = condition,
            Categories = await _categoryService.GetAllFieldsAsync(),
            SelectedCategories = selectedCategories,
            MaxPrice = _productService.GetMaxPrice(),
            MinPrice = _productService.GetMinPrice(),
            SelectedMaxPrice = selectedMaxPrice,
            SelectedMinPrice = selectedMinPrice
        };
        ViewBag.SearchKey = s?.Trim();
        return View(model);
    }

    [Route("Product/{id}/{title}")]
    public async Task<IActionResult> Details(int id, string title)
    {
        if (id < 1)
            return View("NotFound");
        var productDetails = await _productService.GetProductDetails(id);
        if (productDetails is null)
            return View("NotFound");
        return View(productDetails);
    }
}
