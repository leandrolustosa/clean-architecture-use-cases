public class AuditEntry
{
    public AuditEntry(EntityEntry entry)
    {
        Entry = entry;
    }

    public EntityEntry Entry { get; }
    public long Id { get; set; }
    public string TableName { get; set; }
    public IAuditContext AuditContext { get; set; }
    public User User { get; set; }
    public Dictionary<string, string> KeyValues { get; } = new Dictionary<string, string>();
    public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
    public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();
    public List<PropertyEntry> TemporaryProperties { get; } = new List<PropertyEntry>();

    public bool HasTemporaryProperties => TemporaryProperties.Any();

    public LogAcao ToLogAcao(Guid codigoTransacao)
    {
        string propertyName = this.GetDisplayName("Id");
        
        var audit = new LogAcao
        {
            Data = DateTime.Now,
            NomeTabela = TableName,
            ChaveUsuario = User.Chave,
            LoacNmUsuarioAcao = User.Nome,
            IdAcaoSistema = (long)AuditContext.Acao,
            IdFuncionalidadeSistema = (long)AuditContext.Funcionalidade,
            IdSecaoAfretamento = (long?)AuditContext.Secao,
            CodigoRegistro = KeyValues.Count > 0 ? KeyValues[propertyName] : null,
            CodigoTransacao = codigoTransacao.ToString("N"),
            IdRegistro = Id,
            TextoJustificativa = AuditContext.TextoJustificativa
        };

        switch (Entry.State)
        {
            case EntityState.Added:                        
                audit.ItemsLogAcao = NewValues.Select(p => new ItemLogAcao { NomeCampo = p.Key, ValorNovo = Convert.ToString(p.Value) }).ToList();
                break;

            case EntityState.Deleted:
                audit.ItemsLogAcao = OldValues.Select(p => new ItemLogAcao { NomeCampo = p.Key, ValorAntigo = Convert.ToString(p.Value) }).ToList();
                break;

            case EntityState.Modified:
                audit.ItemsLogAcao = NewValues.Select(p => new ItemLogAcao { NomeCampo = p.Key, ValorNovo = Convert.ToString(p.Value), ValorAntigo = Convert.ToString(OldValues[p.Key]) }).ToList();
                break;
        }
        
        return audit;
    }
}