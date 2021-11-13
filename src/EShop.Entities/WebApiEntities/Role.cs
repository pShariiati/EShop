using Microsoft.EntityFrameworkCore;

namespace EShop.Entities.WebApiEntities;

[Index(nameof(Title), IsUnique = true)]
public class Role : BaseEntity
{
    #region Fields

    public string Title { get; set; }

    #endregion

    #region Relations

    public ICollection<User> Users { get; set; }

    #endregion
}
