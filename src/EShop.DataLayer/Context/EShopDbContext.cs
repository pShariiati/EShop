using EShop.Entities;
using EShop.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EShop.DataLayer.Context
{
    public class EShopDbContext :
        IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>,
        IUnitOfWork
    {
        public EShopDbContext(DbContextOptions options)
        : base(options)
        {
        }


        #region Entities

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductProperty> ProductProperties { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartDetail> CartDetails { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }
        public DbSet<ProductProductTag> ProductsProductTags { get; set; }
        public DbSet<UserInformation> UserInformation { get; set; }

        #endregion

        public void MarkAsDeleted<TEntity>(TEntity entity)
            => base.Entry(entity).State = EntityState.Deleted;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(EShopDbContext).Assembly);
        }
    }
}
