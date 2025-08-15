using ECommerce.Application.IServices;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.OrderAggregate;
using ECommerce.Domain.Enums;
using ECommerce.Domain.IRepository;
using Microsoft.Extensions.Configuration;
using Stripe;
namespace ECommerce.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }
        public async Task<Order?> CreateOrUpdatePaymentIntent(int orderId)
        {
            try
            {
                var order = await _unitOfWork.Order.GetAsync(o => o.Id == orderId, includeProperties: "DeliveryMethod");
                if(order == null) 
                    return null;

                StripeConfiguration.ApiKey = _configuration["Stripe:Secretkey"];

                var service = new PaymentIntentService();

                PaymentIntent intent;

                if (string.IsNullOrEmpty(order.PaymentIntentId)) // incase of create PaymentIntent 
                {
                    var options = new PaymentIntentCreateOptions()
                    {
                        Amount = (long)(order.Total * 100),
                        Currency = "USD",
                        PaymentMethodTypes = new List<string>() { "card" },                        
                    };

                    intent = await service.CreateAsync(options);

                    order.PaymentIntentId = intent.Id;
                    order.ClientSecret = intent.ClientSecret;
                }
                else //incase of update
                {
                    var options = new PaymentIntentUpdateOptions()
                    {
                        Amount = (long)(order.Subtotal * 100)
                    };

                    await service.UpdateAsync(order.PaymentIntentId, options);

                }
                order.PaymentMethod = ECommerce.Domain.Enums.PaymentMethod.Stripe;
                await _unitOfWork.Order.UpdateAsync(order);
                await _unitOfWork.SaveAsync();

                return order;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
