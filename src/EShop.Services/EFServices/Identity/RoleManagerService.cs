using EShop.DataLayer.Context;
using EShop.Entities.Identity;
using EShop.Services.Contracts.Identity;
using EShop.ViewModels.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EShop.Services.EFServices.Identity;

public class RoleManagerService
: RoleManager<Role>, IRoleManagerService
{
    private readonly DbSet<Role> _roles;

    public RoleManagerService(
        IRoleStoreService store,
        IEnumerable<IRoleValidator<Role>> roleValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        ILogger<RoleManagerService> logger,
        IUnitOfWork uow
        )
    : base(
        (RoleStore<Role, EShopDbContext, int, UserRole, RoleClaim>)store,
        roleValidators, keyNormalizer, errors, logger
        )
    {
        _roles = uow.Set<Role>();
    }

    public async Task<bool> CheckRolesAsync(List<string> roles)
    {
        var selectedRoles = await _roles.LongCountAsync(x => roles.Contains(x.Name));
        return roles.Count == selectedRoles;
    }

    public Task<List<ShowRole>> GetRolesPreviewAsync()
    {
        return _roles.Select(x => new ShowRole()
        {
            UsersCount = x.UserRoles.Count,
            Id = x.Id,
            Title = x.Name
        }).ToListAsync();
    }

    public Task<EditRoleViewModel> GetForEditAsync(int id)
    {
        return _roles.Select(x => new EditRoleViewModel()
        {
            Id = x.Id,
            Name = x.Name
        }).SingleOrDefaultAsync(x => x.Id == id);
    }

    public Task<Role> RoleToDelete(int id)
        => _roles
            .Where(x => !x.UserRoles.Any())
            .SingleOrDefaultAsync(x => x.Id == id);

    public Task<bool> IsRoleExistsForEdit(int id, string name)
        => _roles.Where(x => x.Name == name)
            .AnyAsync(x => x.Id != id);

    public Task<List<string>> GetRoleNamesAsync()
        => _roles.Select(x => x.Name).ToListAsync();
}
