using Microsoft.AspNetCore.Identity;

namespace EShop.Entities.Identity
{
    public class UserLogin : IdentityUserLogin<int>
    {
        public virtual User User { get; set; }
    }
}
