using System.ComponentModel.DataAnnotations;
using EShop.Common.Constants;
using Microsoft.AspNetCore.Http;

namespace EShop.ViewModels.TestWebApi
{
    public class AddUserViewModel
    {
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
        [MaxLength(100, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
        public string UserName { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
        [MaxLength(50, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
        public string Password { get; set; }

        [Display(Name = "نام و نام خانوادگی")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
        [MaxLength(200, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
        public string FullName { get; set; }

        [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
        public IFormFile UserAvatar { get; set; }

        public string Avatar { get; set; }
    }
}