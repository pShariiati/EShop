using EShop.Entities.Identity;
using EShop.ViewModels.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EShop.Services.Contracts.Identity
{
    public interface IRoleManagerService : IDisposable
    {
        #region BaseClass

        Task<IdentityResult> CreateAsync(Role role);
        Task UpdateNormalizedRoleNameAsync(Role role);
        Task<IdentityResult> UpdateAsync(Role role);
        Task<IdentityResult> DeleteAsync(Role role);
        Task<bool> RoleExistsAsync(string roleName);
        string NormalizeKey(string key);
        Task<Role> FindByIdAsync(string roleId);
        Task<string> GetRoleNameAsync(Role role);
        Task<IdentityResult> SetRoleNameAsync(Role role, string name);
        Task<string> GetRoleIdAsync(Role role);
        Task<Role> FindByNameAsync(string roleName);
        Task<IdentityResult> AddClaimAsync(Role role, Claim claim);
        Task<IdentityResult> RemoveClaimAsync(Role role, Claim claim);
        Task<IList<Claim>> GetClaimsAsync(Role role);
        ILogger Logger { get; set; }
        IList<IRoleValidator<Role>> RoleValidators { get; }
        IdentityErrorDescriber ErrorDescriber { get; set; }
        ILookupNormalizer KeyNormalizer { get; set; }
        IQueryable<Role> Roles { get; }
        bool SupportsQueryableRoles { get; }
        bool SupportsRoleClaims { get; }

        #endregion

        #region CustomMethods

        Task<List<string>> GetRoleNamesAsync();

        Task<bool> CheckRolesAsync(List<string> roles);

        Task<List<ShowRole>> GetRolesPreviewAsync();

        Task<EditRoleViewModel> GetForEditAsync(int id);

        Task<Role> RoleToDelete(int id);

        #endregion
    }
}