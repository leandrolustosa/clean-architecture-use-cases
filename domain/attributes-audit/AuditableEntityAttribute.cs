[AttributeUsage(AttributeTargets.Class)]
public class AuditableEntityAttribute : Attribute
{
    public EnumFuncionalidade Funcionalidade { get; set; }
    public EnumSecao Secao { get; set; }
    public EnumAcao Acao { get; set; }
}