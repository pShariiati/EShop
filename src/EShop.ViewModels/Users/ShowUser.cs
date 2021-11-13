using System.ComponentModel.DataAnnotations;

namespace EShop.ViewModels.Users;

public class ShowUserViewModel
{
    public int Id { get; set; }

    [Display(Name = "نام کاربری")]
    public string UserName { get; set; }

    [Display(Name = "نام و نام خانوادگی")]
    public string FullName { get; set; }

    [Display(Name = "تاریخ عضویت")]
    public DateTime CreatedDateTime { get; set; }

    public bool IsActive { get; set; }
}

public class ShowUsersWithPagination
{
    public List<ShowUserViewModel> Users { get; set; }
    public int CurrentPage { get; set; }
    public int PagesCount { get; set; }
}
