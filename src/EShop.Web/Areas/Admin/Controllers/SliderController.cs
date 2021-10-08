using EShop.Common;
using EShop.Common.Constants;
using EShop.Common.Extensions;
using EShop.DataLayer.Context;
using EShop.Entities;
using EShop.Services.Contracts;
using EShop.ViewModels.Sliders;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace EShop.Web.Areas.Admin.Controllers
{
    [Area(AreaConstants.AdminArea)]
    public class SliderController : BaseController
    {
        private readonly ISliderService _sliderService;
        private readonly IProductService _productService;
        private readonly IUnitOfWork _uow;

        public SliderController(ISliderService sliderService, IUnitOfWork uow, IProductService productService)
        {
            _sliderService = sliderService;
            _uow = uow;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _sliderService.GetPreviewAsync());
        }

        public async Task<IActionResult> Add()
        {
            ViewBag.Products = (await _productService.GetProductForComboBox()).CreateSelectListItem();
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddSliderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Products = (await _productService.GetProductForComboBox()).CreateSelectListItem(selectedItem: model.ProductId);
                ModelState.AddModelError(string.Empty, PublicConstantStrings.ModelStateErrorMessage);
                return View(model);
            }
            var imageExtension = Path.GetExtension(model.Image.FileName);
            var imageName = Guid.NewGuid().ToString("N");
            model.Image.SaveImage(imageName, imageExtension, "sliders");
            await _sliderService.AddAsync(new Slider()
            {
                FirstTitle = model.FirstTitle,
                SecondTitle = model.SecondTitle,
                ProductId = model.ProductId,
                Image = imageName + imageExtension
            });
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (id < 1)
                return View("NotFound");
            var slider = await _sliderService.GetForEdit(id);
            if (slider is null)
                return View("NotFound");
            ViewBag.Products = (await _productService.GetProductForComboBox()).CreateSelectListItem(selectedItem: slider.ProductId);
            return View(slider);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditSliderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Products = (await _productService.GetProductForComboBox()).CreateSelectListItem(selectedItem: model.ProductId);
                ModelState.AddModelError(string.Empty, PublicConstantStrings.ModelStateErrorMessage);
                return View(model);
            }

            var slider = await _sliderService.FindByIdAsync(model.Id);
            if (slider is null)
                return View("NotFound");
            slider.FirstTitle = model.FirstTitle;
            slider.SecondTitle = model.SecondTitle;
            slider.ProductId = model.ProductId;
            if (model.Image != null && model.Image.Length > 0)
            {
                WorkWithImages.RemoveImage(slider.Image, "sliders");
                var imageExtension = Path.GetExtension(model.Image.FileName);
                var imageName = Guid.NewGuid().ToString("N");
                model.Image.SaveImage(imageName, imageExtension, "sliders");
                slider.Image = imageName + imageExtension;
            }
            _sliderService.Update(slider);
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id < 1)
                return View("NotFound");
            var slider = await _sliderService.FindByIdAsync(id);
            if (slider is null)
                return View("NotFound");
            _sliderService.Remove(slider);
            await _uow.SaveChangesAsync();
            WorkWithImages.RemoveImage(slider.Image, "sliders");
            return RedirectToAction(nameof(Index));
        }
    }
}
