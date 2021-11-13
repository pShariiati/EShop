using EShop.Common.Constants;
using EShop.Entities.Identity;
using EShop.Services.Contracts.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace EShop.Services.EFServices.Identity;

public class UserClaimService : UserClaimsPrincipalFactory<User, Role>
{
    public UserClaimService(
        IUserManagerService userManagerService,
        IRoleManagerService roleRoleManagerService,
        IOptions<IdentityOptions> optionsAccessor)
        : base(
            (UserManager<User>)userManagerService,
            (RoleManager<Role>)roleRoleManagerService,
            optionsAccessor
            )
    {

    }

    public override async Task<ClaimsPrincipal> CreateAsync(User user)
    {
        var principal = await base.CreateAsync(user);

        ((ClaimsIdentity)principal.Identity).AddClaims(new[]
        {
                new Claim(IdentityClaimNames.FullName, user.FullName),
                new Claim(IdentityClaimNames.Avatar, user.Avatar),
                //new Claim(ClaimTypes.GivenName, user.FirstName),
            });

        return principal;
    }
}
