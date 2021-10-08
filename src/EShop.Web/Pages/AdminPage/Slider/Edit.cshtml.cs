using EShop.Common;
using EShop.Common.Constants;
using EShop.Common.Extensions;
using EShop.DataLayer.Context;
using EShop.Services.Contracts;
using EShop.ViewModels.Sliders;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace EShop.Web.Pages.AdminPage.Slider
{
    public class EditModel : BasePage
    {
        [BindProperty]
        public EditSliderViewModel Slider { get; set; }

        private readonly ISliderService _sliderService;
        private readonly IProductService _productService;
        private readonly IUnitOfWork _uow;

        public EditModel(ISliderService sliderService, IProductService productService, IUnitOfWork uow)
        {
            _sliderService = sliderService;
            _productService = productService;
            _uow = uow;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id < 1)
                return RedirectToPage("NotFound");
            Slider = await _sliderService.GetForEdit(id);
            if (Slider is null)
                return RedirectToPage("NotFound");
            ViewData["Products"] = (await _productService.GetProductForComboBox()).CreateSelectListItem(selectedItem: Slider.ProductId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["Products"] = (await _productService.GetProductForComboBox()).CreateSelectListItem(selectedItem: Slider.ProductId);
                ModelState.AddModelError(string.Empty, PublicConstantStrings.ModelStateErrorMessage);
                return Page();
            }

            var slider = await _sliderService.FindByIdAsync(Slider.Id);
            if (slider is null)
                return RedirectToPage("NotFound");
            slider.FirstTitle = Slider.FirstTitle;
            slider.SecondTitle = Slider.SecondTitle;
            slider.ProductId = Slider.ProductId;
            if (Slider.Image != null && Slider.Image.Length > 0)
            {
                WorkWithImages.RemoveImage(slider.Image, "sliders");
                var imageExtension = Path.GetExtension(Slider.Image.FileName);
                var imageName = Guid.NewGuid().ToString("N");
                Slider.Image.SaveImage(imageName, imageExtension, "sliders");
                slider.Image = imageName + imageExtension;
            }
            _sliderService.Update(slider);
            await _uow.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}