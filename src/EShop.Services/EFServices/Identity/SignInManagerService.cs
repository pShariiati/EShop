using EShop.Entities.Identity;
using EShop.Services.Contracts.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EShop.Services.EFServices.Identity
{
    public class SignInManagerService
    : SignInManager<User>, ISignInManagerService
    {
        public SignInManagerService(
            IUserManagerService userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<User> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManagerService> logger,
            IAuthenticationSchemeProvider schemes,
            IUserConfirmation<User> confirmation
            )
        : base(
            (UserManager<User>)userManager,
            contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation
            )
        {

        }
    }
}
