using EShop.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Web.Areas.Admin.Controllers;

[Authorize(Roles = IdentityRoleNames.Admin)]
public class BaseController : Controller
{

}
