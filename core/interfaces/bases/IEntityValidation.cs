public interface IEntityValidation<TEntity, TDto>
    where TEntity : Entity
    where TDto : EntityDto
{
    Task<ISingleResult<TEntity>> RegistroExisteAsync(long id);

    Task<ISingleResult<TEntity>> RegistroExisteAsync(long id, params string[] includes);

    Task<ISingleResult<TEntity>> ValidarAsync(TEntity entity);

    Task<ISingleResult<TEntity>> ValidarAsync(TEntity entity, TDto dto);
}