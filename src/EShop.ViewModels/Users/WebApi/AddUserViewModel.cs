﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EShop.Common.Constants;
using Microsoft.AspNetCore.Http;

namespace EShop.ViewModels.Users.WebApi
{
    public class AddUserViewModel
    {
        public string UserName { get; set; }

        public string FullName { get; set; }

        public string Password { get; set; }

        public List<string> Roles { get; set; }

        [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
        public IFormFile Avatar { get; set; }
    }
}