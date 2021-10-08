using Dto.Payment;
using EShop.Common.Constants;
using EShop.Common.Extensions;
using EShop.DataLayer.Context;
using EShop.Entities;
using EShop.Services.Contracts;
using EShop.ViewModels.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using ZarinPal.Class;

namespace EShop.Web.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly Payment _payment;
        private readonly Authority _authority;
        private readonly Transactions _transactions;


        private readonly ICartService _cartService;
        private readonly ICartDetailService _cartDetailService;
        private readonly IProductService _productService;
        private readonly IUnitOfWork _uow;

        public CartController(
            ICartService cartService,
            IUnitOfWork uow, ICartDetailService cartDetailService,
            IProductService productService)
        {
            _cartService = cartService;
            _uow = uow;
            _cartDetailService = cartDetailService;
            _productService = productService;
            var expose = new Expose();
            _payment = expose.CreatePayment();
            _authority = expose.CreateAuthority();
            _transactions = expose.CreateTransactions();
        }


        public async Task<IActionResult> Checkout()
        {
            var userId = User.Identity.GetUserId();
            return View(new CheckoutViewModel()
            {
                UserCartTotalPrice = await _cartDetailService.CalculateUserCartTotalPrice(userId)
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            var userId = User.Identity.GetUserId();
            if (!ModelState.IsValid)
            {
                ViewBag.UserCartTotalPrice = await _cartDetailService.CalculateUserCartTotalPrice(userId);
                ModelState.AddModelError(string.Empty, PublicConstantStrings.ModelStateErrorMessage);
                return View(model);
            }

            var userCart = await _cartService.GetUserCartAsync(userId);
            if (userCart is null || userCart.TotalPrice <= 0)
                return RedirectToAction(nameof(HomeController.Index), "Home");
            userCart.Address = model.Address;
            await _uow.SaveChangesAsync();
            var payment = new ZarinPal.Class.Payment();

            var result = await _payment.Request(new DtoRequest()
            {
                Mobile = "09121112222",
                CallbackUrl = Url.Action("PaymentResult", "Cart", new { area = "", orderId = userCart.Id }, protocol: Request.Scheme),
                Description = "توضیحات",
                Email = "farazmaan@outlook.com",
                Amount = userCart.TotalPrice + 15000,
                MerchantId = "XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX"
            }, ZarinPal.Class.Payment.Mode.sandbox);

            if (result.Status == 100)
            {
                return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + result.Authority);
            }

            return View("Error");
        }

        public async Task<IActionResult> PaymentResult(int orderId, string status, string authority)
        {
            if (string.IsNullOrWhiteSpace(status) || string.IsNullOrWhiteSpace(authority))
                return View("Error");
            var model = new PaymentResultViewModel();
            if (status.Equals("OK", StringComparison.OrdinalIgnoreCase))
            {
                var userCart = await _cartService.FindByIdAsync(orderId);
                if (userCart is null)
                {
                    return View("Error");
                }

                var verification = await _payment.Verification(new DtoVerification
                {
                    Amount = userCart.TotalPrice + 15000,
                    MerchantId = "XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX",
                    Authority = authority
                }, Payment.Mode.sandbox);
                model.IsPay = verification.Status == 100;
                if (model.IsPay)
                {
                    model.TotalPrice = (userCart.TotalPrice + 15000).ToString("#,0");
                    //model.RefId = userCart.RefId = verification.RefId;
                    model.RefId = verification.RefId;

                    //Update user cart
                    userCart.RefId = verification.RefId;
                    userCart.IsPay = true;
                    await _uow.SaveChangesAsync();
                }
                else if (verification.Status == 101)
                {
                    ViewBag.Message = "این صورتحساب قبلا تایید شده است.";
                }
            }
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int productId)
        {
            var product = await _productService.FindByIdAsync(productId);
            if (product is null)
                return BadRequest();
            var userId = User.Identity.GetUserId();
            var userCart = await _cartService.GetUserCartAsync(userId);
            if (userCart is null)
            {
                userCart = new Cart()
                {
                    UserId = userId
                };
                await _cartService.AddAsync(userCart);
            }

            var cartDetail = await _cartDetailService.GetCartDetailsBy(productId, userId);
            if (cartDetail is null)
            {
                userCart.CartDetails.Add(new CartDetail()
                {
                    ProductId = productId,
                    Count = 1,
                    Price = product.Price
                });
            }
            else
            {
                cartDetail.Count++;
            }
            //await _uow.SaveChangesAsync();
            userCart.TotalPrice = (await _cartDetailService.CalculateUserCartTotalPrice(userId)) +
                                  product.Price;
            await _uow.SaveChangesAsync();
            return Json(userCart.TotalPrice.ToString("#,0"));
        }

        //#episode 67
        public async Task<string> GetUserCartTotalPrice()
        {
            var userId = User.Identity.GetUserId();
            return (await _cartDetailService.CalculateUserCartTotalPrice(userId)).ToString("#,0");
        }

        public async Task<PartialViewResult> ShowCartDetailsPreview()
        {
            var userId = User.Identity.GetUserId();
            var model = new ShowCartDetailsViewModel()
            {
                CartDetails = await _cartDetailService.GetCartDetailsBy(userId)
            };
            model.UserCartTotalPrice =
                model.CartDetails.Sum(x => x.Price * x.Count)
                    .ToString("#,0");
            return PartialView("_CartDetailsPartial", model);
        }

        public async Task<IActionResult> IncreaseOrLowOff(int productId, bool isIncrease, bool removeAll)
        {
            var product = await _productService.FindByIdAsync(productId);
            if (product is null)
                return BadRequest();
            var userId = User.Identity.GetUserId();
            var cartDetail = await _cartDetailService.FindBy(productId, userId);
            if (cartDetail is null)
                return BadRequest();
            if (removeAll)
            {
                _cartDetailService.Remove(cartDetail);
            }
            else if (isIncrease)
            {
                cartDetail.Count++;
            }
            else
            {
                if (cartDetail.Count <= 1)
                    _cartDetailService.Remove(cartDetail);
                else
                    cartDetail.Count--;
            }
            var userCart = await _cartService.GetUserCartAsync(userId);
            if (isIncrease)
            {
                userCart.TotalPrice = await _cartDetailService.CalculateUserCartTotalPrice(userId)
                    + product.Price;
            }
            else
            {
                userCart.TotalPrice = await _cartDetailService.CalculateUserCartTotalPrice(userId)
                                      - product.Price * (removeAll ? cartDetail.Count : 1);
            }
            await _uow.SaveChangesAsync();
            return Ok();
        }

        public async Task<IActionResult> MyCarts()
        {
            var userId = User.Identity.GetUserId();
            var model = await _cartService.GetUserCartsForClient(userId);
            return View(model);
        }

        public async Task<IActionResult> ShowCartDetails(int id)
        {
            var userId = User.Identity.GetUserId();
            var cartDetails = await _cartDetailService.GetCartDetails(userId, id);
            return View(cartDetails);
        }
    }
}
