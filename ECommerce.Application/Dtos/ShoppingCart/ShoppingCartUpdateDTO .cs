namespace ECommerce.Application.Dtos.ShoppingCart
{
    public class ShoppingCartUpdateDTO
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int ProductId { get; set; }
    }
}
