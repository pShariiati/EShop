using Microsoft.AspNetCore.Identity;

namespace EShop.Entities.Identity
{
    public class RoleClaim : IdentityRoleClaim<int>
    {
        public virtual Role Role { get; set; }
    }
}
