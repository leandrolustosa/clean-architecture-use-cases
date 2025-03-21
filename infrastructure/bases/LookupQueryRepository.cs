public abstract class LookupQueryRepository<TEntity> : Repository<TEntity>, ILookupQueryRepository<TEntity>
	where TEntity : Entity
{	
	public LookupQueryRepository(AplicationDbContext context) : base(context)
	{	
	}

	public abstract IQueryable<TEntity> GetLookupQuery();
}