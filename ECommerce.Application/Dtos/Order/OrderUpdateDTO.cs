namespace ECommerce.Application.Dtos.Order
{
    public class OrderUpdateDTO
    {
        public int Id { get; set; }
        public ICollection<int> ProductId { get; set; }
        public decimal SubTotal { get; set; }
        public DateTimeOffset CreationDateTime { get; set; }
    }
}
