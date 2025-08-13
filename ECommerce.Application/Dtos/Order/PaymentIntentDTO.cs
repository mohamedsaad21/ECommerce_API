using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;

namespace ECommerce.Application.Dtos.Order
{
    public class PaymentIntentDTO
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public string OrderStatus { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public Address ShippingAddress { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
    }
}
