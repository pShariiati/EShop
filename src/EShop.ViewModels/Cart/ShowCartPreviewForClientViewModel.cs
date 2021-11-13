using System.ComponentModel.DataAnnotations;

namespace EShop.ViewModels.Cart;

public class ShowCartPreviewForClientViewModel
{
    public int Id { get; set; }

    [Display(Name = "مجموع قیمت")]
    public int TotalPrice { get; set; }

    [Display(Name = "وضعیت پرداخت")]
    public bool IsPay { get; set; }

    [Display(Name = "شماره پیگیری")]
    public int RefId { get; set; }

    [Display(Name = "آدرس")]
    public string Address { get; set; }
}
public class ShowCartPreviewForAdminViewModel
{
    public int Id { get; set; }

    [Display(Name = "نام مشتری")]

    public string CustomerFullName { get; set; }

    [Display(Name = "مجموع قیمت")]
    public int TotalPrice { get; set; }

    [Display(Name = "وضعیت پرداخت")]
    public bool IsPay { get; set; }

    [Display(Name = "شماره پیگیری")]
    public int RefId { get; set; }

    [Display(Name = "آدرس")]
    public string Address { get; set; }
}
