using ECommerce.Application.IServices;
using ECommerce.Domain.Entities;
using ECommerce.Domain.IRepository;
namespace ECommerce.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IPaymentService _paymentService;
        private readonly IUnitOfWork _unitOfWork;
        public OrderService(IUnitOfWork unitOfWork, IPaymentService paymentService)
        {
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }
        public async Task<Order>? CreateOrder(string UserId)
        {
            var cart = await _unitOfWork.ShoppingCart.GetAllAsync(u => u.ApplicationUserId == UserId);
            var products = await _unitOfWork.Product.GetAllAsync();

            var orderProducts = products.Join(cart, p => p.Id, c => c.ProductId, (p, c) => new
            {
                ProductId = p.Id,
                p.Price,
                c.Count
            }).ToList();
            var order = new Order
            {
                ApplicationUserId = UserId!,
                SubTotal = orderProducts.Select(p => p.Price * p.Count).Sum(),
                OrderStatus = OrderStatus.Pending
            };
            await _unitOfWork.Order.CreateAsync(order);
            await _unitOfWork.SaveAsync();
            // Reduce Stock For Each Product In Cart
            foreach (var item in cart)
            {
                var product = await _unitOfWork.Product.GetAsync(u => u.Id == item.ProductId);
                if(product is not null)
                {
                    if (product.Stock >= item.Count)
                    {
                        product.Stock -= item.Count;
                        await _unitOfWork.ShoppingCart.RemoveAsync(item);
                    }
                    else return null; // Not Enough Stock
                }
            }
            var result = await _paymentService.CreateOrUpdatePaymentIntent(order);
            return result ? order : null;
        }
    }
}
