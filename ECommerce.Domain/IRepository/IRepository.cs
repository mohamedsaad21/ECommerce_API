using System.Linq.Expressions;
namespace ECommerce.Domain.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, int pageSize = 3, int pageNumber = 1);
        Task<T> GetAsync(Expression<Func<T, bool>> filter, bool tracked = true, string? includeProperties = null);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task RemoveAsync(T entity);
    }
}
