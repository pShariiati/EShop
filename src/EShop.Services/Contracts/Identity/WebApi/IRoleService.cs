using EShop.Entities.WebApiEntities;

namespace EShop.Services.Contracts.Identity.WebApi;

public interface IRoleService : IGenericService<Role>
{
    List<Role> GetRolesBy(List<string> roles);
}
