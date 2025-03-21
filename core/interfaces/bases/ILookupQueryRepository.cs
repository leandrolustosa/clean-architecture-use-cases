public interface ILookupQueryRepository<TEntity> : IRepository<TEntity>
        where TEntity : Entity
{   
    IQueryable<TEntity> GetLookupQuery();
}