using System.Linq.Expressions;

namespace ToDoList.Model.Data.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<T?> GetWithIncludesAsync(Guid id, params Expression<Func<T, object>>[] includes);
    }
}

