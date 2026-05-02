using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ToDoList.Model.Data.DatabaseProvider;

namespace ToDoList.Model.Data.Repositories
{
    internal class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContextOptions<ApplicationContex> _options;

        public Repository(DbContextOptions<ApplicationContex> options)
        {
            _options = options;
        }

        private ApplicationContex CreateContext() => new ApplicationContex(_options);

        public async virtual Task<IEnumerable<T>> GetAllAsync()
        {
            using (var context = CreateContext()) 
            {
                return await context.Set<T>().AsNoTracking().ToListAsync();
            } 
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            using (var context = CreateContext())
            {
                return await context.Set<T>().FindAsync(id);
            }
        }

        public async Task AddAsync(T entity)
        {
            using (var context = CreateContext())
            {
                await context.Set<T>().AddAsync(entity);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(T entity)
        {
            using (var context = CreateContext())
            {
                context.Set<T>().Update(entity);
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(T entity)
        {
            using (var context = CreateContext())
            {
                context.Set<T>().Remove(entity);
                await context.SaveChangesAsync();
            }
        }

        public async Task<T?> GetWithIncludesAsync(Guid id, params Expression<Func<T, object>>[] includes)
        {
            using(var context = CreateContext())
            {
                IQueryable<T> query = context.Set<T>();

                foreach (var include in includes)
                {
                    query = query.Include(include);
                }

                return await query.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id);
            }
        }
    }
}
