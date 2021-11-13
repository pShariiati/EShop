using System.ComponentModel.DataAnnotations;

namespace EShop.ViewModels.Products;

public class ShowProductViewModel
{
    public int Id { get; set; }

    [Display(Name = "عنوان")]
    public string Title { get; set; }

    [Display(Name = "قیمت")]
    public int Price { get; set; }

    [Display(Name = "دسته بندی")]
    public string CategoryTitle { get; set; }

    public bool CanRemove { get; set; }
}
