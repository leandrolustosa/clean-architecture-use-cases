public class AuditableEntityDto : EntityDto, IAuditableEntity
{
    public IAuditContext AuditContext { get; set; }

    public User User { get; set; }
    public string TextoJustificativaAuditable { get; set; }
    public bool InibirMensagens { get; set; }
}