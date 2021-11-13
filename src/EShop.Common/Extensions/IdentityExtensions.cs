using EShop.Common.Constants;
using System.Security.Claims;
using System.Security.Principal;

namespace EShop.Common.Extensions;

public static class IdentityExtensions
{
    public static string FindFirstValue(this ClaimsIdentity identity, string claimType)
    {
        return identity?.FindFirst(claimType)?.Value;
    }

    public static string GetUserFullName(this IIdentity identity)
    {
        var claimsIdentity = identity as ClaimsIdentity;
        //var firstName = claimsIdentity?.FindFirstValue(ClaimTypes.GivenName);
        //var lastName = claimsIdentity?.FindFirstValue(ClaimTypes.Surname);
        //return $"{firstName} {lastName}";
        return claimsIdentity.FindFirstValue(IdentityClaimNames.FullName);
    }

    public static int GetUserId(this IIdentity identity)
    {
        var claimsIdentity = identity as ClaimsIdentity;
        return int.Parse(claimsIdentity.FindFirstValue(ClaimTypes.NameIdentifier));
    }

    public static string GetUserAvatar(this IIdentity identity)
    {
        var claimsIdentity = identity as ClaimsIdentity;
        return claimsIdentity.FindFirstValue(IdentityClaimNames.Avatar);
    }
}
