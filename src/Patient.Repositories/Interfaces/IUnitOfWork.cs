namespace Patient.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> GetRepository<T>() where T : class;

        Task<int> SaveChangesAsync();
    }
}
