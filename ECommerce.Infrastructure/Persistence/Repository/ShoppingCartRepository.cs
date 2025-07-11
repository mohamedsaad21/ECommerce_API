using ECommerce.Domain.Entities;
using ECommerce.Domain.IRepository;
namespace ECommerce.Infrastructure.Persistence.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly ApplicationDbContext _db;

        public ShoppingCartRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
