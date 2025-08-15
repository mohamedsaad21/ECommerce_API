using ECommerce.Application.Dtos.Order;
using ECommerce.Domain.Entities.OrderAggregate;
namespace ECommerce.Application.IServices
{
    public interface IOrderService
    {
        Task<Order>? CreateOrder(string UserId, OrderCreateDTO orderDTO);
    }
}
