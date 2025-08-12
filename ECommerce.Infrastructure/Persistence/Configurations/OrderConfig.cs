using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations
{
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasMany(o => o.OrderItems).WithOne(i => i.Order).HasForeignKey(i => i.OrderId);

            builder.OwnsOne(o => o.ShippingAddress, a =>
            {
                a.Property(a => a.FirstName).HasColumnName("FirstName");
                a.Property(a => a.LastName).HasColumnName("LastName");
                a.Property(a => a.Street).HasColumnName("Street");
                a.Property(a => a.City).HasColumnName("City");
                a.Property(a => a.Country).HasColumnName("Country");
            });
        }
    }
}
