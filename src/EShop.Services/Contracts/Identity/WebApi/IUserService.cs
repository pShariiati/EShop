using EShop.ViewModels.Account.WebApi;
using EShop.ViewModels.Users.WebApi;

namespace EShop.Services.Contracts.Identity.WebApi
{
    public interface IUserService
    {
        UserToBuildJwtTokenViewModel GetUserBy(LoginViewModel model);
    }
}
