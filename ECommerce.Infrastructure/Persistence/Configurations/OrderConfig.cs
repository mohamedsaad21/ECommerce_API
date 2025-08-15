using ECommerce.Domain.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations
{
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasMany(o => o.OrderItems).WithOne(i => i.Order).HasForeignKey(i => i.OrderId);

            builder.HasOne(o => o.OrderDelivery).WithOne(d => d.order).HasForeignKey<OrderDelivery>(d => d.OrderId);
            builder.HasOne(o => o.DeliveryMethod).WithMany(m => m.Orders).HasForeignKey(o => o.DeliveryMethodId);
        }
    }
}
