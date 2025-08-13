namespace ECommerce.Application.Dtos.Order
{
    public class OrderItemDTO
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string ImageUrl { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
    }
}
