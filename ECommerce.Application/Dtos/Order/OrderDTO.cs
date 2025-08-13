using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;

namespace ECommerce.Application.Dtos.Order
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; } 
        public string OrderStatus { get; set; } 
        public DateTimeOffset OrderDate { get; set; } 
        public List<OrderItemDTO> OrderItems { get; set; } 
        public Address ShippingAddress { get; set; }
    }
}
