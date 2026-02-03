using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using ToDoList.Model.Data.DatabaseProvider;
using ToDoList.Model.Data.POCO;

namespace ToDoList.Model.Data.Repositories
{
    internal class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationContex _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(ApplicationContex context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async virtual Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
        
        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }
        public async Task AddAsync(T entity )
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task LoadEntryAsync<TProperty>(T entity, Expression<Func<T, IEnumerable<TProperty>>> navigationProperty)
        where TProperty : class
        {
            await _context.Entry(entity).Collection(navigationProperty).LoadAsync();
        }
    }
}
