using EShop.Common;
using EShop.Common.Constants;
using EShop.Common.Extensions;
using EShop.DataLayer.Context;
using EShop.Services.Contracts;
using EShop.ViewModels.Sliders;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Web.Pages.AdminPage.Slider;

public class AddModel : BasePage
{
    private readonly IProductService _productService;
    private readonly ISliderService _sliderService;
    private IUnitOfWork _uow;
    public AddSliderViewModel Slider { get; set; }

    public AddModel(IProductService productService, ISliderService sliderService, IUnitOfWork uow)
    {
        _productService = productService;
        _sliderService = sliderService;
        _uow = uow;
    }

    public async Task OnGetAsync()
    {
        ViewData["Products"] = (await _productService.GetProductForComboBox()).CreateSelectListItem();
    }

    public async Task<IActionResult> OnPostAsync(AddSliderViewModel slider)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Products"] = (await _productService.GetProductForComboBox()).CreateSelectListItem(selectedItem: slider.ProductId);
            ModelState.AddModelError(string.Empty, PublicConstantStrings.ModelStateErrorMessage);
            return Page();
        }
        var imageExtension = Path.GetExtension(slider.Image.FileName);
        var imageName = Guid.NewGuid().ToString("N");
        slider.Image.SaveImage(imageName, imageExtension, "sliders");
        await _sliderService.AddAsync(new Entities.Slider()
        {
            FirstTitle = slider.FirstTitle,
            SecondTitle = slider.SecondTitle,
            ProductId = slider.ProductId,
            Image = imageName + imageExtension
        });
        await _uow.SaveChangesAsync();
        return RedirectToPage("./Index");
    }
}
