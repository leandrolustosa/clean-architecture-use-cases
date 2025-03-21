public interface IRepository<TEntity> : IDisposable
        where TEntity : Entity
{
    IQueryable<TEntity> Set();
    Task AddAsync(TEntity obj);
    Task AddRangeAsync(IEnumerable<TEntity> obj);
    TEntity GetById(long id);
    TEntity GetById(long id, bool tracking);
    Task<TEntity> GetByIdAsync(long id);
    Task<TEntity> GetByIdAsync(long id, params string[] includes);
    IQueryable<LookupEntity> GetLookup();
    IQueryable<TEntity> GetAll();
    IQueryable<TEntity> GetAll(params string[] includes);
    void Update(TEntity obj);
    void Remove(long id);
    void Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> obj, IAuditContext auditContext, User user);
    Task<int> SaveChangesAsync();
    IQueryable<TEntity> Buscar(Expression<Func<TEntity, bool>> predicate);

}