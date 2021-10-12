using EShop.ViewModels.Users.WebApi;

namespace EShop.Services.Contracts.Identity.WebApi
{
    public interface ITokenService
    {
        string BuildToken(string key, string issuer, UserToBuildJwtTokenViewModel user, bool rememberMe);
        bool IsTokenValid(string key, string issuer, string token);
    }
}
