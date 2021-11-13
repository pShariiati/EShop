using System.ComponentModel.DataAnnotations;

namespace EShop.ViewModels.Categories;

public class ShowCategory
{
    public int Id { get; set; }

    [Display(Name = "عنوان")]
    public string Title { get; set; }

    public bool CanRemove { get; set; }
}
