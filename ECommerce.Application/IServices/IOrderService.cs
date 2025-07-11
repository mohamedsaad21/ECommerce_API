using ECommerce.Domain.Entities;
namespace ECommerce.Application.IServices
{
    public interface IOrderService
    {
        Task<Order>? CreateOrder(string UserId);
    }
}
