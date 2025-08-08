namespace ECommerce.Application.Dtos.ShoppingCart
{
    public class ShoppingCartCreateDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
