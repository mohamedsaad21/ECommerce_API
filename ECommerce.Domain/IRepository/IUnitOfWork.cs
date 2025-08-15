using ECommerce.Domain.Entities.OrderAggregate;

namespace ECommerce.Domain.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }
        IOrderRepository Order { get; }
        IShoppingCartRepository ShoppingCart { get; }

        IRepository<DeliveryMethod> DeliveryMethodsRepository { get; }
        Task SaveAsync();
    }
}
