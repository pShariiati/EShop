namespace EShop.Services.Contracts
{
    public interface ICookieManager
    {
        public void Add(string cookieName, string cookieValue);
        public string GetValue(string cookieName);
    }
}
