using System.ComponentModel.DataAnnotations;

namespace EShop.ViewModels.Sliders;

public class ShowSliderViewModel
{
    public int Id { get; set; }

    [Display(Name = "محصول")]
    public string ProductTitle { get; set; }

    [Display(Name = "عنوان اول")]
    public string FirstTitle { get; set; }

    [Display(Name = "عنوان دوم")]
    public string SecondTitle { get; set; }
}
