using ECommerce.Application.Dtos;
namespace ECommerce.Application.IServices
{
    public interface IAuthService
    {
        Task<AuthModel> RegisterAsync(RegisterModel model);
        Task<AuthModel> GetTokenAsync(TokenRequestModel model);
        Task<string> AssignToRoleAsync(AddRoleModel model);
    }
}
