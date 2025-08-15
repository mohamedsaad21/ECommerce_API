using AutoMapper;
using ECommerce.Application.Dtos.Order;
using ECommerce.Application.IServices;
using ECommerce.Domain.Entities.OrderAggregate;
using ECommerce.Domain.IRepository;
using ECommerce.Domain.Enums;
namespace ECommerce.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Order>? CreateOrder(string UserId, OrderCreateDTO orderDTO)
        {
            var cart = await _unitOfWork.ShoppingCart.GetAsync(u => u.ApplicationUserId == UserId, includeProperties: "ShoppingCartItems");
            var deliveryMethod = await _unitOfWork.DeliveryMethodsRepository.GetAsync(m => m.Id == orderDTO.DeliveryMethodId);
            if(cart == null || deliveryMethod == null)
            {
                return null;
            }
            var subtotal = cart.ShoppingCartItems.Select(i => i.UnitPrice * i.Quantity).Sum();

            var order = new Order
            {
                ApplicationUserId = UserId!,
                Subtotal = subtotal,
                DeliveryMethod = deliveryMethod,
                DeliveryMethodId = orderDTO.DeliveryMethodId,
                OrderDelivery = new OrderDelivery
                {
                    ShippingAddress = _mapper.Map<Address>(orderDTO.ShippingAddress),
                    DeliveryStatus = DeliveryStatus.Pending                    
                }                
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
                    }
                    else return null; // Not Enough Stock
                }
            }
            await _unitOfWork.ShoppingCart.RemoveAsync(cart);
            await _unitOfWork.SaveAsync();
            return order;
        }
    }
}
