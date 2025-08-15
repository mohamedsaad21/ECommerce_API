namespace ECommerce.Domain.Entities.OrderAggregate
{
    public class DeliveryMethod
    {

        public int Id { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public string DeliveryTime { get; set; }
        public List<Order> Orders { get; set; }
    }
}
