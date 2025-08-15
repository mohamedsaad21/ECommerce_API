namespace ECommerce.Application.Dtos.Order
{
    public class OrderCreateDTO
    {
        public int DeliveryMethodId { get; set; }
        public AddressDTO ShippingAddress { get; set; }
    }
}
