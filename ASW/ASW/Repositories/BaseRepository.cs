using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ASW.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ASW.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly DbContext _context;

        public BaseRepository(DbContext context)
        {
            _context = context;
        }

        public async Task Insert(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Set<T>().Update(entity);
        }

        public void Remove(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Set<T>().Remove(entity);
        }

        public async Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public async Task<T> Find(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().FindAsync(predicate);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}