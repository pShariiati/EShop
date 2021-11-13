using EShop.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace EShop.ViewModels.TestWebApi;

public class LoginViewModel
{
    [Display(Name = "نام کاربری")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [MaxLength(10, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string UserName { get; set; }

    [Display(Name = "رمز عبور")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [MaxLength(20, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string Password { get; set; }

    [Display(Name = "مرا به خاطر بسپار")]
    public bool RememberMe { get; set; }
}
