using Microsoft.AspNetCore.Mvc;

namespace EShop.Web.Areas.Admin.Controllers;

[Area(AreaConstants.AdminArea)]
public class HomeController : BaseController
{
    public IActionResult Index()
    {
        return View();
    }
}
