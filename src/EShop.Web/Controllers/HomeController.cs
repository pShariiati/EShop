using System;
using EShop.DataLayer.Context;
using EShop.Services.Contracts;
using EShop.Services.Contracts.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using EShop.Common.Security;
using Microsoft.AspNetCore.Http;

namespace EShop.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IUserManagerService _userManagerService;
        private readonly IProductImageService _productImageService;
        private readonly IUnitOfWork _uow;
        private readonly ILogger<HomeController> _logger;
        private readonly ICookieManager _cookieManager;

        public HomeController(
            ICategoryService categoryService,
            ILogger<HomeController> logger,
            IUserManagerService userManagerService,
            IUnitOfWork uow,
            IProductImageService productImageService,
            ICookieManager cookieManager)
        {
            _categoryService = categoryService;
            _logger = logger;
            _userManagerService = userManagerService;
            _uow = uow;
            _productImageService = productImageService;
            _cookieManager = cookieManager;
        }

        public async Task<IActionResult> Index()
        {
            //_cookieManager.Add("Test Name", "Test value", new CookieOptions
            //{
            //    SameSite = SameSiteMode.Lax,
            //    Expires = DateTimeOffset.Now.AddMinutes(10),
            //    Secure = true,
            //    Path = "",
            //    HttpOnly = true,
            //    Domain = "",
            //    IsEssential = true,
            //});
            //_logger.LogTrace("Log trace test");
            //_logger.LogInformation("Log information test");
            //_logger.LogWarning("Log warning test");
            //throw new Exception("Exception test");
            //var user = await _userManagerService.FindByIdAsync(5.ToString());
            //user.UserInformation = new UserInformation()
            //{
            //    FullName = "Payam Shariati",
            //    BirthDate = new DateTime(1990, 1, 1),
            //    EyeColor = EyeColor.Brown,
            //    NationalCode = "1002003001"
            //};
            //await _uow.SaveChangesAsync();
            var categories = await _categoryService.GetAllFieldsAsync();
            return View(categories);
        }
    }
}