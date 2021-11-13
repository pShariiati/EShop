using EShop.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShop.DataLayer.Configurations;

public class ProductProductTagConfiguration : IEntityTypeConfiguration<ProductProductTag>
{
    public void Configure(EntityTypeBuilder<ProductProductTag> builder)
    {
        builder.HasKey(x => new { x.ProductId, x.ProductTagId });
    }
}
