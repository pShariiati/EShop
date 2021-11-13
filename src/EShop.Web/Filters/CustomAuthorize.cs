using EShop.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EShop.Web.Filters;

public class CustomAuthorize : IAuthorizationFilter
{
    private readonly ICookieManager _cookieManager;

    public CustomAuthorize(ICookieManager cookieManager)
    {
        _cookieManager = cookieManager;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var token = _cookieManager.GetValue("JWTToken");
        var hasAllowAnonymous = context.ActionDescriptor.EndpointMetadata
            .Any(em => em.GetType() == typeof(AllowAnonymousAttribute));
        if (string.IsNullOrWhiteSpace(token) && !hasAllowAnonymous)
        {
            context.Result = new RedirectToActionResult("Login", "WebApi", null);
        }
    }
}
