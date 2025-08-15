using ECommerce.Domain.Entities.OrderAggregate;

namespace ECommerce.Application.IServices
{
    public interface IPaymentService
    {
        Task<Order?> CreateOrUpdatePaymentIntent(int orderId);
    }
}
