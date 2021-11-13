using EShop.Common.Constants;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EShop.ViewModels.Cart;

public class CheckoutViewModel
{
    [Display(Name = "آدرس")]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [MaxLength(300, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string Address { get; set; }

    [HiddenInput]
    [Range(1, int.MaxValue, ErrorMessage = AttributesErrorMessages.RangeMessage)]
    public int UserCartTotalPrice { get; set; }
}
