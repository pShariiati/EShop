using System.ComponentModel.DataAnnotations;

namespace EShop.ViewModels.TestWebApi;

public class ShowUserViewModel
{
    [Display(Name = "نام کاربری")]
    public string UserName { get; set; }

    [Display(Name = "نام و نام خانوادگی")]
    public string FullName { get; set; }

    [Display(Name = "عکس کاربر")]
    public string Avatar { get; set; }
}
