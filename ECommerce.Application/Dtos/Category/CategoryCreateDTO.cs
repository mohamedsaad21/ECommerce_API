using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.Dtos.Category
{
    public class CategoryCreateDTO
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }
    }
}
