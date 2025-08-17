using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.Dtos.Product
{
    public class ProductDTO
    {
        public int Id { get; set; }

        [Required, MaxLength(250)]
        public string Name { get; set; }

        [Required, MaxLength(600)]
        public string Description { get; set; }

        [Required, Precision(16, 2)]
        public decimal Price { get; set; }

        [Required]
        public int stock { get; set; }

        public List<string>? images { get; set; }

        public int CategoryId { get; set; }

        public double AverageRating { get; set; }

    }
}
