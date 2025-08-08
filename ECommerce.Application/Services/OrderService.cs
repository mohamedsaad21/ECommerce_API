using ECommerce.Application.IServices;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
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
            var cart = await _unitOfWork.ShoppingCart.GetAsync(u => u.ApplicationUserId == UserId, includeProperties: "ShoppingCartItems");
            
            var order = new Order
            {
                ApplicationUserId = UserId!,
                TotalAmount = 0,
                OrderStatus = OrderStatus.Pending,
                PaymentStatus = PaymentStatus.Unpaid,
            };
           await _unitOfWork.Order.CreateAsync(order);
            var orderItems = cart.ShoppingCartItems.Select(o => new OrderItem
            {
                ProductId = o.ProductId,
                ProductName = o.ProductName,
                ImageUrl = o.ImageUrl,
                OrderId = order.Id,
                UnitPrice = o.UnitPrice,
                Quantity = o.Quantity
            });
            //var orderProducts = products.Join(cart, p => p.Id, c => c.ProductId, (p, c) => new
            //{
            //    ProductId = p.Id,
            //    p.Price,
            //    c.Count
            //}).ToList();
            //var order = new Order
            //{
            //    ApplicationUserId = UserId!,
            //    TotalAmount = orderProducts.Select(p => p.Price * p.Count).Sum(),
            //    OrderStatus = OrderStatus.Pending
            //};
            order.OrderItems.AddRange(orderItems);
            // Reduce Stock For Each Product In Cart
            foreach (var item in orderItems)
            {
                var product = await _unitOfWork.Product.GetAsync(u => u.Id == item.ProductId);
                if(product is not null)
                {
                    if (product.Stock >= item.Quantity)
                    {
                        product.Stock -= item.Quantity;
                        //await _unitOfWork.ShoppingCart.RemoveAsync(item);
                    }
                    else return null; // Not Enough Stock
                }
            }
            var result = await _paymentService.CreateOrUpdatePaymentIntent(order);
            await _unitOfWork.SaveAsync();
            return result ? order : null;
        }
    }
}
