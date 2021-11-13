using EShop.Entities.Identity;
using EShop.ViewModels.Account;
using EShop.ViewModels.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace EShop.Services.Contracts.Identity;

public interface IUserManagerService : IDisposable
{
    #region BaseClass

    string GetUserName(ClaimsPrincipal principal);
    string GetUserId(ClaimsPrincipal principal);
    Task<User> GetUserAsync(ClaimsPrincipal principal);
    Task<string> GenerateConcurrencyStampAsync(User user);
    Task<IdentityResult> CreateAsync(User user);
    Task<IdentityResult> CreateAsync(User user, string password);
    Task<IdentityResult> UpdateAsync(User user);
    Task<IdentityResult> DeleteAsync(User user);
    Task<User> FindByIdAsync(string userId);
    Task<User> FindByNameAsync(string userName);
    string NormalizeName(string name);
    string NormalizeEmail(string email);
    Task UpdateNormalizedUserNameAsync(User user);
    Task<string> GetUserNameAsync(User user);
    Task<IdentityResult> SetUserNameAsync(User user, string userName);
    Task<string> GetUserIdAsync(User user);
    Task<bool> CheckPasswordAsync(User user, string password);
    Task<bool> HasPasswordAsync(User user);
    Task<IdentityResult> AddPasswordAsync(User user, string password);
    Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword);
    Task<IdentityResult> RemovePasswordAsync(User user);
    Task<string> GetSecurityStampAsync(User user);
    Task<IdentityResult> UpdateSecurityStampAsync(User user);
    Task<string> GeneratePasswordResetTokenAsync(User user);
    Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword);
    Task<User> FindByLoginAsync(string loginProvider, string providerKey);
    Task<IdentityResult> RemoveLoginAsync(User user, string loginProvider, string providerKey);
    Task<IdentityResult> AddLoginAsync(User user, UserLoginInfo login);
    Task<IList<UserLoginInfo>> GetLoginsAsync(User user);
    Task<IdentityResult> AddClaimAsync(User user, Claim claim);
    Task<IdentityResult> AddClaimsAsync(User user, IEnumerable<Claim> claims);
    Task<IdentityResult> ReplaceClaimAsync(User user, Claim claim, Claim newClaim);
    Task<IdentityResult> RemoveClaimAsync(User user, Claim claim);
    Task<IdentityResult> RemoveClaimsAsync(User user, IEnumerable<Claim> claims);
    Task<IList<Claim>> GetClaimsAsync(User user);
    Task<IdentityResult> AddToRoleAsync(User user, string role);
    Task<IdentityResult> AddToRolesAsync(User user, IEnumerable<string> roles);
    Task<IdentityResult> RemoveFromRoleAsync(User user, string role);
    Task<IdentityResult> RemoveFromRolesAsync(User user, IEnumerable<string> roles);
    Task<IList<string>> GetRolesAsync(User user);
    Task<bool> IsInRoleAsync(User user, string role);
    Task<string> GetEmailAsync(User user);
    Task<IdentityResult> SetEmailAsync(User user, string email);
    Task<User> FindByEmailAsync(string email);
    Task UpdateNormalizedEmailAsync(User user);
    Task<string> GenerateEmailConfirmationTokenAsync(User user);
    Task<IdentityResult> ConfirmEmailAsync(User user, string token);
    Task<bool> IsEmailConfirmedAsync(User user);
    Task<string> GenerateChangeEmailTokenAsync(User user, string newEmail);
    Task<IdentityResult> ChangeEmailAsync(User user, string newEmail, string token);
    Task<string> GetPhoneNumberAsync(User user);
    Task<IdentityResult> SetPhoneNumberAsync(User user, string phoneNumber);
    Task<IdentityResult> ChangePhoneNumberAsync(User user, string phoneNumber, string token);
    Task<bool> IsPhoneNumberConfirmedAsync(User user);
    Task<string> GenerateChangePhoneNumberTokenAsync(User user, string phoneNumber);
    Task<bool> VerifyChangePhoneNumberTokenAsync(User user, string token, string phoneNumber);
    Task<bool> VerifyUserTokenAsync(User user, string tokenProvider, string purpose, string token);
    Task<string> GenerateUserTokenAsync(User user, string tokenProvider, string purpose);
    void RegisterTokenProvider(string providerName, IUserTwoFactorTokenProvider<User> provider);
    Task<IList<string>> GetValidTwoFactorProvidersAsync(User user);
    Task<bool> VerifyTwoFactorTokenAsync(User user, string tokenProvider, string token);
    Task<string> GenerateTwoFactorTokenAsync(User user, string tokenProvider);
    Task<bool> GetTwoFactorEnabledAsync(User user);
    Task<IdentityResult> SetTwoFactorEnabledAsync(User user, bool enabled);
    Task<bool> IsLockedOutAsync(User user);
    Task<IdentityResult> SetLockoutEnabledAsync(User user, bool enabled);
    Task<bool> GetLockoutEnabledAsync(User user);
    Task<DateTimeOffset?> GetLockoutEndDateAsync(User user);
    Task<IdentityResult> SetLockoutEndDateAsync(User user, DateTimeOffset? lockoutEnd);
    Task<IdentityResult> AccessFailedAsync(User user);
    Task<IdentityResult> ResetAccessFailedCountAsync(User user);
    Task<int> GetAccessFailedCountAsync(User user);
    Task<IList<User>> GetUsersForClaimAsync(Claim claim);
    Task<IList<User>> GetUsersInRoleAsync(string roleName);
    Task<string> GetAuthenticationTokenAsync(User user, string loginProvider, string tokenName);
    Task<IdentityResult> SetAuthenticationTokenAsync(User user, string loginProvider, string tokenName, string tokenValue);
    Task<IdentityResult> RemoveAuthenticationTokenAsync(User user, string loginProvider, string tokenName);
    Task<string> GetAuthenticatorKeyAsync(User user);
    Task<IdentityResult> ResetAuthenticatorKeyAsync(User user);
    string GenerateNewAuthenticatorKey();
    Task<IEnumerable<string>> GenerateNewTwoFactorRecoveryCodesAsync(User user, int number);
    Task<IdentityResult> RedeemTwoFactorRecoveryCodeAsync(User user, string code);
    Task<int> CountRecoveryCodesAsync(User user);
    Task<byte[]> CreateSecurityTokenAsync(User user);
    ILogger Logger { get; set; }
    IPasswordHasher<User> PasswordHasher { get; set; }
    IList<IUserValidator<User>> UserValidators { get; }
    IList<IPasswordValidator<User>> PasswordValidators { get; }
    ILookupNormalizer KeyNormalizer { get; set; }
    IdentityErrorDescriber ErrorDescriber { get; set; }
    IdentityOptions Options { get; set; }
    bool SupportsUserAuthenticationTokens { get; }
    bool SupportsUserAuthenticatorKey { get; }
    bool SupportsUserTwoFactorRecoveryCodes { get; }
    bool SupportsUserTwoFactor { get; }
    bool SupportsUserPassword { get; }
    bool SupportsUserSecurityStamp { get; }
    bool SupportsUserRole { get; }
    bool SupportsUserLogin { get; }
    bool SupportsUserEmail { get; }
    bool SupportsUserPhoneNumber { get; }
    bool SupportsUserClaim { get; }
    bool SupportsUserLockout { get; }
    bool SupportsQueryableUsers { get; }
    IQueryable<User> Users { get; }

    #endregion

    #region CustomMethods
    ShowUsersWithPagination UsersWithPaginationPreview(int selectedPage, int tak = 1);
    Task<EditUserViewModel> GetUserForEditAsync(int id);
    Task<EditAccountViewModel> GetUserForEditAccountAsync(int id);

    #endregion
}
