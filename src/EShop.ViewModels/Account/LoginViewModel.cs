using EShop.Common.Constants;
using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EShop.ViewModels.Account
{
    public class LoginViewModel// : GoogleReCaptchaModelBase
    {
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
        [MinLength(3, ErrorMessage = AttributesErrorMessages.MinLengthMessage)]
        [MaxLength(30, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
        public string UserName { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
        //[DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "به خاطر سپاری رمز عبور ؟")]
        public bool RememberMe { get; set; }

        public List<AuthenticationScheme> ExternalLogins { get; set; }
    }
}