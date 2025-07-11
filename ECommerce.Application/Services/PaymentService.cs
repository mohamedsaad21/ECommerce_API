﻿using ECommerce.Application.IServices;
using ECommerce.Domain.Entities;
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
        public async Task<bool> CreateOrUpdatePaymentIntent(Order order)
        {
            try
            {
                StripeConfiguration.ApiKey = _configuration["Stripe:Secretkey"];


                if (order == null) return false;


                var service = new PaymentIntentService();

                PaymentIntent intent;

                if (string.IsNullOrEmpty(order.PaymentIntentId)) // incase of create PaymentIntent 
                {
                    var options = new PaymentIntentCreateOptions()
                    {
                        Amount = (long)(order.SubTotal * 100),
                        Currency = "usd",
                        PaymentMethodTypes = new List<string>() { "card" }
                    };

                    intent = await service.CreateAsync(options);

                    order.PaymentIntentId = intent.Id;
                    order.ClientSecret = intent.ClientSecret;
                }
                else //incase of update
                {
                    var options = new PaymentIntentUpdateOptions()
                    {
                        Amount = (long)(order.SubTotal * 100)
                    };

                    await service.UpdateAsync(order.PaymentIntentId, options);

                }
                order.OrderStatus = OrderStatus.PaymentReceived;
                await _unitOfWork.Order.UpdateAsync(order);
                await _unitOfWork.SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
