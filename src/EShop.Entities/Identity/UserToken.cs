using Microsoft.AspNetCore.Identity;

namespace EShop.Entities.Identity
{
    public class UserToken : IdentityUserToken<int>
    {
        public virtual User User { get; set; }
    }
}
