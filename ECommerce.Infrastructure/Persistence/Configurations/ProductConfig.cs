using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace ECommerce.Infrastructure.Persistence.Configurations
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name).IsRequired().HasMaxLength(250);
            builder.Property(p => p.Description).IsRequired().HasMaxLength(600);
            builder.Property(p => p.Price).IsRequired().HasPrecision(16, 2);
            builder.Property(p => p.Stock).IsRequired();
            builder.Property(p => p.ImagesPath).IsRequired(false);

            builder.HasOne(p => p.Category).WithMany(c => c.Products);
            builder.HasMany(p => p.Orders).WithMany(o => o.Products);
            builder.HasMany(p => p.ShoppingCarts).WithOne(c => c.Product);
        }
    }
}
