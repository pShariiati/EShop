using EShop.Services.Contracts;
using Microsoft.AspNetCore.Http;

namespace EShop.Services
{
    public class CookieManager : ICookieManager
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public CookieManager(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public void Add(string cookieName, string cookieValue)
        {
            _contextAccessor.HttpContext.Response.Cookies.Append(cookieName, cookieValue);
        }

        public string GetValue(string cookieName)
        {
            _contextAccessor.HttpContext.Request.Cookies.TryGetValue(cookieName, out var value);
            return value;
        }
    }
}