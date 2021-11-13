using EShop.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Web.ViewComponents;

public class BestSellingProductsViewComponent : ViewComponent
{
    private readonly IProductService _productService;

    public BestSellingProductsViewComponent(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View("", await _productService.GetBestSellingProductAsync());
    }
}
