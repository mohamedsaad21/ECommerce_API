using System.ComponentModel.DataAnnotations;
namespace ECommerce.Application.Dtos
{
    public class TokenRequestModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
