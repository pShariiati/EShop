using EShop.Entities.WebApiEntities;
using Microsoft.EntityFrameworkCore;

namespace EShop.DataLayer.Context;

public class TicketDbContext : DbContext, IUnitOfWork
{
    public TicketDbContext(DbContextOptions options)
    : base(options)
    {
    }


    #region Entities

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

    #endregion

    public void MarkAsDeleted<TEntity>(TEntity entity)
        => base.Entry(entity).State = EntityState.Deleted;
}
