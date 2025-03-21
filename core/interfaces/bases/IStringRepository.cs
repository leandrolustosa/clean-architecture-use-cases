public interface IStringRepository<TEntity> : IDisposable
        where TEntity : StringEntity
{
    IQueryable<TEntity> Set();
    Task AddAsync(TEntity obj);
    Task AddRangeAsync(IEnumerable<TEntity> obj);
    TEntity GetById(string id);
    TEntity GetById(string id, bool tracking);
    Task<TEntity> GetByIdAsync(string id);
    Task<TEntity> GetByIdAsync(string id, params string[] includes);
    IQueryable<LookupStringEntity> GetLookup();
    IQueryable<TEntity> GetAll();
    IQueryable<TEntity> GetAll(params string[] includes);
    void Update(TEntity obj);
    void Remove(string id);
    void Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> obj, IAuditContext auditContext, User user);
    Task<int> SaveChangesAsync();
    IQueryable<TEntity> Buscar(Expression<Func<TEntity, bool>> predicate);

}