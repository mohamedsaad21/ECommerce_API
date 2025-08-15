using ECommerce.Domain.Enums;
namespace ECommerce.Domain.Entities.OrderAggregate
{
    public class OrderDelivery
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order order { get; set; }
        public Address ShippingAddress { get; set; }
        public string? TrackingNumber { get; set; }
        public DeliveryStatus DeliveryStatus { get; set; }
    }
}
