using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Entities.OrderAggregate
{
    public class Order
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total=> Subtotal + DeliveryMethod.Cost;
        public int DeliveryMethodId { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.NotSpecified;
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
        public OrderDelivery OrderDelivery { get; set; }
    }
}
