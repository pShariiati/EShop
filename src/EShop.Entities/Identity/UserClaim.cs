using Microsoft.AspNetCore.Identity;

namespace EShop.Entities.Identity
{
    public class UserClaim : IdentityUserClaim<int>
    {
        public virtual User User { get; set; }
    }
}
