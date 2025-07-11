using ECommerce.Domain.Entities;
using ECommerce.Domain.IRepository;
namespace ECommerce.Infrastructure.Persistence.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
