using Microsoft.AspNetCore.Http;

namespace EShop.Services.Contracts;

public interface ICookieManager
{
    public void Add(string cookieName, string cookieValue, CookieOptions options = null);
    public string GetValue(string cookieName);
}
