using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EShop.Common.Constants;

namespace EShop.ViewModels.Users.WebApi
{
    public class AddUserViewModelBase64
    {
        [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
        [MaxLength(100, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
        public string UserName { get; set; }

        public string FullName { get; set; }

        public string Password { get; set; }

        public List<string> Roles { get; set; }

        public string Avatar { get; set; }
    }
}