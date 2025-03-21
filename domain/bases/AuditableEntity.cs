public class AuditableEntity : Entity, IAuditableEntity
{
    public IAuditContext AuditContext { get; set; }

    public User User { get; set; }
}