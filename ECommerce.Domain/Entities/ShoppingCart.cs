namespace ECommerce.Domain.Entities
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public List<ShoppingCartItem> ShoppingCartItems { get; set; } = new List<ShoppingCartItem>();
    }
}
