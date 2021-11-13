using EShop.Common.Constants;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EShop.ViewModels.Users;

public class EditUserViewModel
{
    [HiddenInput]
    public int Id { get; set; }

    [Display(Name = "نام کاربری")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [MinLength(3, ErrorMessage = AttributesErrorMessages.MinLengthMessage)]
    [MaxLength(50, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    [Remote("CheckUserName", "Account", null,
        ErrorMessage = AttributesErrorMessages.RemoteMessage, HttpMethod = "POST", AdditionalFields = ViewModelConstants.AntiForgeryToken + "," + nameof(Id))]
    [RegularExpression(@"^\w+$", ErrorMessage = "نام کاربری باید از حروف انگلیسی تشکیل شده باشد")]
    public string UserName { get; set; }

    [Display(Name = "ایمیل")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$", ErrorMessage = AttributesErrorMessages.RegularExpressionMessage)]
    [MaxLength(100, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    [Remote("CheckEmail", "Account", null,
        ErrorMessage = AttributesErrorMessages.RemoteMessage, HttpMethod = "POST", AdditionalFields = ViewModelConstants.AntiForgeryToken + "," + nameof(Id))]
    public string Email { get; set; }

    [Display(Name = "رمز عبور")]
    [MinLength(6, ErrorMessage = AttributesErrorMessages.MinLengthMessage)]
    [MaxLength(50, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "تکرار رمز عبور")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = AttributesErrorMessages.CompareMessage)]
    public string ConfirmPassword { get; set; }

    [Display(Name = "نام")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [MinLength(3, ErrorMessage = AttributesErrorMessages.MinLengthMessage)]
    [MaxLength(50, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    [RegularExpression(@"^[\u0600-\u06FF,\u0590-\u05FF\s]*$",
        ErrorMessage = "لطفا تنها از حروف فارسی استفاده نمائید")]
    public string FirstName { get; set; }

    [Display(Name = "نام خانوادگی")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [MinLength(3, ErrorMessage = AttributesErrorMessages.MinLengthMessage)]
    [MaxLength(50, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    [RegularExpression(@"^[\u0600-\u06FF,\u0590-\u05FF\s]*$",
        ErrorMessage = "لطفا تنها از حروف فارسی استفاده نمائید")]
    public string LastName { get; set; }

    [Display(Name = "نقش های کاربر")]
    public List<string> SelectedRoles { get; set; }
        = new List<string>();
}
