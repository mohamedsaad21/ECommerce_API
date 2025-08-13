using ECommerce.Domain.Entities;

namespace ECommerce.Application.IServices
{
    public interface IPaymentService
    {
        Task<Order?> CreateOrUpdatePaymentIntent(int orderId);
    }
}
