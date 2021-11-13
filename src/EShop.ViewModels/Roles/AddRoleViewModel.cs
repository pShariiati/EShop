﻿using EShop.Common.Constants;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EShop.ViewModels.Roles;

public class AddRoleViewModel
{
    [Display(Name = "عنوان")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [MaxLength(100, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    [Remote("CheckRoleNameForAdd", "Role", "Admin",
        AdditionalFields = ViewModelConstants.AntiForgeryToken,
        ErrorMessage = AttributesErrorMessages.RemoteMessage, HttpMethod = "POST")]
    public string Name { get; set; }
}
