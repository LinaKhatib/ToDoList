using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ToDoList.Model.Data.Repositories
{
    internal interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task SaveAsync();
        Task LoadEntryAsync<TProperty>(T entity, Expression<Func<T, IEnumerable<TProperty>>> navigationProperty) where TProperty : class;
    }
}
