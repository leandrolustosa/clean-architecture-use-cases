public interface IUnitOfWork : IDisposable
{
    Task BeginTransactionAsync();
    Task<bool> CommitAsync();
    void Rollback();
}