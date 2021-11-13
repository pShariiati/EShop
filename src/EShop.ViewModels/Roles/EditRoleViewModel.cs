using EShop.Common.Constants;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EShop.ViewModels.Roles;

public class EditRoleViewModel
{
    [HiddenInput]
    public int Id { get; set; }

    [Display(Name = "عنوان")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [MaxLength(100, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    [Remote("CheckRoleNameForEdit", "Role", "Admin",
        AdditionalFields = ViewModelConstants.AntiForgeryToken + "," + nameof(Id),
        ErrorMessage = AttributesErrorMessages.RemoteMessage, HttpMethod = "POST")]
    public string Name { get; set; }
}
