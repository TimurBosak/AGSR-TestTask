using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Patient.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(params object[] id);

        Task<IReadOnlyCollection<T>> GetAllAsync();

        Task<IReadOnlyCollection<T>> GetWhereAsync(Expression<Func<T, bool>> predicate);

        void Add(T entity);

        void AddRange(IEnumerable<T> entities);

        void Update(T entity);

        void Remove(T entity);

        IQueryable<T> GetQuery();
     }
}
