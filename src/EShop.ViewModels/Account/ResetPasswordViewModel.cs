using EShop.Common.Constants;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EShop.ViewModels.Account;

public class ResetPasswordViewModel
{
    [Display(Name = "ایمیل")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$", ErrorMessage = AttributesErrorMessages.RegularExpressionMessage)]
    [MaxLength(100, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string Email { get; set; }

    [Display(Name = "رمز عبور")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [MinLength(6, ErrorMessage = AttributesErrorMessages.MinLengthMessage)]
    [MaxLength(50, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "تکرار رمز عبور")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = AttributesErrorMessages.CompareMessage)]
    public string ConfirmPassword { get; set; }

    [HiddenInput]
    public string Token { get; set; }
}
