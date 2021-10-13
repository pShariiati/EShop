using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace EShop.Entities.WebApiEntities
{
    [Index(nameof(UserName), IsUnique = true)]
    public class User : BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(100)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; }

        public ICollection<Role> Roles { get; set; }
            = new List<Role>();
    }
}