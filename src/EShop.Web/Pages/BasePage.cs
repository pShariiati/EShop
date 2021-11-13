using EShop.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EShop.Web.Pages;

[Authorize(Roles = IdentityRoleNames.Admin)]
public class BasePage : PageModel
{

}
