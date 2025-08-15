using ECommerce.Domain.Entities.OrderAggregate;
namespace ECommerce.Application.Dtos.Order
{
    public class OrderToReturnDTO
    {
        public int Id { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public string DeliveryMethod { get; set; } 
        public string Status { get; set; } 
        public DateTimeOffset OrderDate { get; set; } 
        public List<OrderItemDTO> OrderItems { get; set; } 
        public Address ShippingAddress { get; set; }
        public string? PaymentIntentId { get; set; }
    }
}
