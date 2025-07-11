using System.Linq.Expressions;
namespace ECommerce.Domain.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }
        IOrderRepository Order { get; }
        IShoppingCartRepository ShoppingCart { get; }
        Task SaveAsync();
    }
}
