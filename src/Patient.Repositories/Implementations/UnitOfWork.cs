using Microsoft.EntityFrameworkCore;
using Patient.Repositories.Interfaces;

namespace Patient.Repositories.Implementations
{
    public class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        private readonly DbContext _context;

        private readonly Dictionary<Type, Type> _entityRepositoryMapping;
        private readonly Dictionary<Type, object> _repositories;


        public UnitOfWork(TContext context)
        {
            _context = context;

            _entityRepositoryMapping = new Dictionary<Type, Type>();
            _repositories = new Dictionary<Type, object>();
        }


        public IRepository<T> GetRepository<T>() where T : class
        {
            if (_repositories.TryGetValue(typeof(T), out var repository))
            {
                return (IRepository<T>)repository;
            }
            repository = CreateRepository<T>();
            _repositories.Add(typeof(T), repository);

            return (IRepository<T>)repository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }


        protected void RegisterCustomRepository<TEntity, TRepository>()
            where TEntity : class
            where TRepository : IRepository<TEntity>
        {
            _entityRepositoryMapping.Add(typeof(TEntity), typeof(TRepository));
        }


        private IRepository<T> CreateRepository<T>() where T : class
        {
            if (!_entityRepositoryMapping.TryGetValue(typeof(T), out var repositoryType))
            {
                var repository = new Repository<T>(_context);

                return repository;
            }
            var customRepository = Activator.CreateInstance(repositoryType, _context);

            return (IRepository<T>)customRepository;
        }
    }
}
