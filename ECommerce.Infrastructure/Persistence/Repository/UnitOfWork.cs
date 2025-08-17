using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.OrderAggregate;
using ECommerce.Domain.IRepository;

namespace ECommerce.Infrastructure.Persistence.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public ICategoryRepository Category {  get; private set; }
        public IProductRepository Product { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IOrderRepository Order { get; private set; }
        public IRepository<DeliveryMethod> DeliveryMethodsRepository { get; private set; }
        public IRepository<Feedback> Feedbacks { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Product = new ProductRepository(_db);
            ShoppingCart = new ShoppingCartRepository(_db);
            Order = new OrderRepository(_db);
            DeliveryMethodsRepository = new Repository<DeliveryMethod>(_db);
            Feedbacks = new Repository<Feedback>(_db);
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
