using EShop.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace EShop.Services.Contracts.Identity
{
    public interface IUserStoreService : IDisposable
    {
        Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken);
        Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken);
        Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken);
        Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken);
        Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken);
        Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken);
        Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken);
        Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken);
        Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken);
        int ConvertIdFromString(string id);
        string ConvertIdToString(int id);
        Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken);
        Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken);
        Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken);
        Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken);
        Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancellationToken);
        Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken);
        Task ReplaceClaimAsync(User user, Claim claim, Claim newClaim, CancellationToken cancellationToken);
        Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken);
        Task AddLoginAsync(User user, UserLoginInfo login, CancellationToken cancellationToken);
        Task RemoveLoginAsync(User user, string loginProvider, string providerKey, CancellationToken cancellationToken);
        Task<IList<UserLoginInfo>> GetLoginsAsync(User user, CancellationToken cancellationToken);
        Task<User> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken);
        Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancellationToken);
        Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken);
        Task SetEmailAsync(User user, string email, CancellationToken cancellationToken);
        Task<string> GetEmailAsync(User user, CancellationToken cancellationToken);
        Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancellationToken);
        Task SetNormalizedEmailAsync(User user, string normalizedEmail, CancellationToken cancellationToken);
        Task<User> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken);
        Task<DateTimeOffset?> GetLockoutEndDateAsync(User user, CancellationToken cancellationToken);
        Task SetLockoutEndDateAsync(User user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken);
        Task<int> IncrementAccessFailedCountAsync(User user, CancellationToken cancellationToken);
        Task ResetAccessFailedCountAsync(User user, CancellationToken cancellationToken);
        Task<int> GetAccessFailedCountAsync(User user, CancellationToken cancellationToken);
        Task<bool> GetLockoutEnabledAsync(User user, CancellationToken cancellationToken);
        Task SetLockoutEnabledAsync(User user, bool enabled, CancellationToken cancellationToken);
        Task SetPhoneNumberAsync(User user, string phoneNumber, CancellationToken cancellationToken);
        Task<string> GetPhoneNumberAsync(User user, CancellationToken cancellationToken);
        Task<bool> GetPhoneNumberConfirmedAsync(User user, CancellationToken cancellationToken);
        Task SetPhoneNumberConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken);
        Task SetSecurityStampAsync(User user, string stamp, CancellationToken cancellationToken);
        Task<string> GetSecurityStampAsync(User user, CancellationToken cancellationToken);
        Task SetTwoFactorEnabledAsync(User user, bool enabled, CancellationToken cancellationToken);
        Task<bool> GetTwoFactorEnabledAsync(User user, CancellationToken cancellationToken);
        Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken);
        Task SetTokenAsync(User user, string loginProvider, string name, string value, CancellationToken cancellationToken);
        Task RemoveTokenAsync(User user, string loginProvider, string name, CancellationToken cancellationToken);
        Task<string> GetTokenAsync(User user, string loginProvider, string name, CancellationToken cancellationToken);
        Task SetAuthenticatorKeyAsync(User user, string key, CancellationToken cancellationToken);
        Task<string> GetAuthenticatorKeyAsync(User user, CancellationToken cancellationToken);
        Task<int> CountCodesAsync(User user, CancellationToken cancellationToken);
        Task ReplaceCodesAsync(User user, IEnumerable<string> recoveryCodes, CancellationToken cancellationToken);
        Task<bool> RedeemCodeAsync(User user, string code, CancellationToken cancellationToken);
        IdentityErrorDescriber ErrorDescriber { get; set; }
        IQueryable<User> Users { get; }
        bool AutoSaveChanges { get; set; }
        Task<IList<User>> GetUsersInRoleAsync(string normalizedRoleName, CancellationToken cancellationToken);
        Task AddToRoleAsync(User user, string normalizedRoleName, CancellationToken cancellationToken);
        Task RemoveFromRoleAsync(User user, string normalizedRoleName, CancellationToken cancellationToken);
        Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken);
        Task<bool> IsInRoleAsync(User user, string normalizedRoleName, CancellationToken cancellationToken);
    }
}