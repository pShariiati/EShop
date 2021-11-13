using EShop.Common.Extensions;
using EShop.DataLayer.Context;
using EShop.Services.Contracts;
using EShop.ViewModels.Sliders;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Web.Pages.AdminPage.Slider;

public class IndexModel : BasePage
{
    public List<ShowSliderViewModel> Sliders { get; set; }

    private readonly ISliderService _sliderService;
    private IUnitOfWork _uow;

    public IndexModel(ISliderService sliderService, IUnitOfWork uow)
    {
        _sliderService = sliderService;
        _uow = uow;
    }

    public async Task OnGetAsync()
    {
        Sliders = await _sliderService.GetPreviewAsync();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        if (id < 1)
            return RedirectToPage("NotFound");
        var slider = await _sliderService.FindByIdAsync(id);
        if (slider is null)
            return RedirectToPage("NotFound");
        _sliderService.Remove(slider);
        await _uow.SaveChangesAsync();
        WorkWithImages.RemoveImage(slider.Image, "sliders");
        return RedirectToPage("./Index");
    }
}
