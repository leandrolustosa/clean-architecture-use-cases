public class AuditableEntityMap<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : AuditableEntity
{        
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.Ignore(d => d.AuditContext);

        builder.Ignore(d => d.User);
    }
}
