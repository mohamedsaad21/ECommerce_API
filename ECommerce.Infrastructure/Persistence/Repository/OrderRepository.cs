using ECommerce.Domain.Entities.OrderAggregate;
using ECommerce.Domain.IRepository;
namespace ECommerce.Infrastructure.Persistence.Repository
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
