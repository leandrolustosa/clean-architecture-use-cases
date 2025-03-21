public interface IAuditableEntity : IEntity<long>
{
    IAuditContext AuditContext { get; set; }

    User User { get; set; }
}