using EShop.DataLayer.Context;
using EShop.Entities.WebApiEntities;
using EShop.Services.Contracts.Identity.WebApi;
using Microsoft.EntityFrameworkCore;

namespace EShop.Services.EFServices.Identity.WebApi;

public class RoleService : GenericService<Role>, IRoleService
{
    private readonly DbSet<Role> _roles;
    public RoleService(IUnitOfWork uow) : base(uow)
    {
        _roles = uow.Set<Role>();
    }

    public List<Role> GetRolesBy(List<string> roles)
        => _roles
            .Where(x => roles.Contains(x.Title))
            .ToList();
}
