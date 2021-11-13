using EShop.Entities.WebApiEntities;
using EShop.ViewModels.Account.WebApi;
using EShop.ViewModels.Users.WebApi;

namespace EShop.Services.Contracts.Identity.WebApi;

public interface IUserService : IGenericService<User>
{
    UserToBuildJwtTokenViewModel GetUserBy(LoginViewModel model);
    bool IsExistsByUserNameForAdd(string userName);
    bool IsExistsByUserNameForEdit(string userName, int userId);
    User GetUserToEdit(int userId);
}
