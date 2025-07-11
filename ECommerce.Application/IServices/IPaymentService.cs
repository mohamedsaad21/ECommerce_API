using ECommerce.Domain.Entities;

namespace ECommerce.Application.IServices
{
    public interface IPaymentService
    {
        Task<bool> CreateOrUpdatePaymentIntent(Order order);
    }
}
