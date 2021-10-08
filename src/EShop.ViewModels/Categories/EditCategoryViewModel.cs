using EShop.Common.Constants;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EShop.ViewModels.Categories
{
    public class EditCategoryViewModel
    {
        [HiddenInput]
        public int Id { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
        [MaxLength(100, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
        public string Title { get; set; }

        [Display(Name = "زیر دسته")]
        public int? ParentId { get; set; }
    }
}
