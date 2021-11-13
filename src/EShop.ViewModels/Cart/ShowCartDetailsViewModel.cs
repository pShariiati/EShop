using System.ComponentModel.DataAnnotations;

namespace EShop.ViewModels.Cart;

public class ShowCartDetailsViewModel
{
    public string UserCartTotalPrice { get; set; }
    public List<CartDetailPreviewViewModel> CartDetails { get; set; }
}

public class CartDetailPreviewViewModel
{
    public int ProductId { get; set; }

    [Display(Name = "عنوان محصول")]
    public string ProductTitle { get; set; }

    [Display(Name = "عکس محصول")]
    public string ProductImage { get; set; }

    [Display(Name = "تعداد")]
    public int Count { get; set; }

    [Display(Name = "قیمت واحد")]
    public int Price { get; set; }
    //public int TotalPrice
    //{
    //    get
    //    {
    //        return Count * Price;
    //    }
    //}

    [Display(Name = "مجموع قیمت")]
    public int TotalPrice => Price * Count;
}
public class CartDetailPreviewForAdminViewModel
{
    [Display(Name = "نام مشتری")]
    public string CustomerFullName { get; set; }
    public int ProductId { get; set; }

    [Display(Name = "عنوان محصول")]
    public string ProductTitle { get; set; }

    [Display(Name = "عکس محصول")]
    public string ProductImage { get; set; }

    [Display(Name = "تعداد")]
    public int Count { get; set; }

    [Display(Name = "قیمت واحد")]
    public int Price { get; set; }
    //public int TotalPrice
    //{
    //    get
    //    {
    //        return Count * Price;
    //    }
    //}

    [Display(Name = "مجموع قیمت")]
    public int TotalPrice => Price * Count;
}
