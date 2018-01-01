using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ASW.Repositories.Contracts
{
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