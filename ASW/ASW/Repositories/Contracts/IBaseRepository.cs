using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ASW.Repositories.Contracts
{
    /// <summary>
    /// Base contract for Repository Pattern, providing the data storage operations (CRUD/Queries) to easily scale for new entities (which are received by generics - T), 
    /// * It Prevents duplicated code
    /// * It Keeps code clean, improving reuse
    /// </summary>
    /// <typeparam name="T">Entity to be handled</typeparam>
    public interface IBaseRepository<T> where T : class
    {
        Task Insert(T entity);
        void Update(T entity);
        void Remove(T entity);
        Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate);
        Task<T> Find(Expression<Func<T, bool>> predicate);
        void SaveChanges();
        Task SaveChangesAsync();
    }
}