using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EShop.Entities.WebApiEntities;

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

    [MaxLength(50)]
    public string Avatar { get; set; }

    public ICollection<Role> Roles { get; set; }
        = new List<Role>();
}
