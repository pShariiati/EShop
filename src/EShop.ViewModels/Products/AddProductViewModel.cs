using EShop.Common.Constants;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EShop.ViewModels.Products
{
    public class AddProductViewModel
    {
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
        [MaxLength(200, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
        public string Description { get; set; }

        [Display(Name = "قیمت")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
        public int Price { get; set; }

        [Display(Name = "دسته بندی")]
        public int CategoryId { get; set; }

        [Display(Name = "زیر دسته")]
        [Range(1, int.MaxValue, ErrorMessage = "لطفا دسته بندی را انتخاب نمایید")]
        public int CategoryChildrenId { get; set; }

        [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
        [Display(Name = "عکس های محصول")]
        public List<IFormFile> Images { get; set; }

        [Display(Name = "ویژگی های محصول")]
        public List<string> Properties { get; set; }
            = new List<string>();

        [Display(Name = "کلمات کلیدی")]
        public string Tags { get; set; }
    }
}
