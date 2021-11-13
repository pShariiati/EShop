using EShop.Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace EShop.ViewModels.Account;

public class ForgotPasswordViewModel
{
    [Display(Name = "ایمیل")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$", ErrorMessage = AttributesErrorMessages.RegularExpressionMessage)]
    [MaxLength(100, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string Email { get; set; }
}
