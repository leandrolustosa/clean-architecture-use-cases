public interface ISingleResult<TEntity> : IResult
	where TEntity : Entity
{	
	TEntity Data { get; set; }
}