using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace EShop.Entities.Identity
{
    public class Role : IdentityRole<int>
    {
        public Role(string name)
        : base(name)
        {

        }
        public virtual ICollection<RoleClaim> RoleClaims { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
