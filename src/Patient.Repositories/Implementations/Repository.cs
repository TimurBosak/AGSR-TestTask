using Microsoft.EntityFrameworkCore;
using Patient.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Patient.Repositories.Implementations
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected DbContext dbContext;
        protected DbSet<T> dbSet;


        public Repository(DbContext context)
        {
            dbContext = context;
            dbSet = context.Set<T>();
        }


        public async Task<T> GetByIdAsync(params object[] id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<IReadOnlyCollection<T>> GetAllAsync()
        {
            return await GetQuery().ToListAsync();
        }

        public async Task<IReadOnlyCollection<T>> GetWhereAsync(Expression<Func<T, bool>> predicate)
        {
            return await GetQuery().Where(predicate).ToListAsync();
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            dbSet.AddRange(entities);
        }

        public void Remove(T entity)
        {
            dbSet.Attach(entity);
            dbSet.Remove(entity);
        }

        public void Update(T entity)
        {
            dbSet.Attach(entity);
            dbSet.Update(entity);
        }

        public IQueryable<T> GetQuery()
        {
            return dbSet.AsQueryable();
        }
    }
}
