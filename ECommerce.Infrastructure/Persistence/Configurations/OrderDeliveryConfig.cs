using ECommerce.Domain.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations
{
    public class OrderDeliveryConfig : IEntityTypeConfiguration<OrderDelivery>
    {
        public void Configure(EntityTypeBuilder<OrderDelivery> builder)
        {
            builder.OwnsOne(o => o.ShippingAddress, a =>
            {
                a.Property(a => a.FirstName).HasColumnName("FirstName").IsRequired();
                a.Property(a => a.LastName).HasColumnName("LastName").IsRequired();
                a.Property(a => a.Street).HasColumnName("Street").IsRequired();
                a.Property(a => a.City).HasColumnName("City").IsRequired();
                a.Property(a => a.ZipCode).HasColumnName("ZipCode").IsRequired();
                a.Property(a => a.State).HasColumnName("State").IsRequired();
            });
        }
    }
}
