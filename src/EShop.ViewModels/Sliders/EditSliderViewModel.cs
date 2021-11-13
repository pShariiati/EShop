using EShop.Common.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EShop.ViewModels.Sliders;

public class EditSliderViewModel
{
    [HiddenInput]
    public int Id { get; set; }

    [Display(Name = "محصول")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [Range(1, int.MaxValue, ErrorMessage = "لطفا محصول را انتخاب کنید")]
    public int ProductId { get; set; }

    [Display(Name = "عنوان اول")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [MaxLength(50, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string FirstTitle { get; set; }

    [Display(Name = "عنوان دوم")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [MaxLength(50, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string SecondTitle { get; set; }

    [Display(Name = "عکس اسلایدر")]
    public IFormFile Image { get; set; }
}
