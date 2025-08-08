using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;

namespace ECommerce.Application.Dtos.Order
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Stripe;
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
        public List<OrderItem> OrderItems { get; set; }
    }
}
