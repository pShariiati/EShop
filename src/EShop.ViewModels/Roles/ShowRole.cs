using System.ComponentModel.DataAnnotations;

namespace EShop.ViewModels.Roles;

public class ShowRole
{
    public int Id { get; set; }

    [Display(Name = "عنوان")]
    public string Title { get; set; }

    [Display(Name = "تعداد کاربران در این نقش")]
    public int UsersCount { get; set; }
}
