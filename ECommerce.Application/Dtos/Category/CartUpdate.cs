using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.Dtos.Category
{
    public class CartUpdate
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
    }
}
