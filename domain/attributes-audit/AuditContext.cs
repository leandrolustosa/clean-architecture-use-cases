public class AuditContext : IAuditContext
{
    public string Identificador { get; set; }
    public EnumFuncionalidade Funcionalidade { get; set; }
    public EnumSecao? Secao { get; set; }
    public EnumAcao Acao { get; set; }
    public string TextoJustificativa { get; set; }

    #region Clones

    public IAuditContext Clone()
    {
        var identificador = (string)this.Identificador.Clone();

        return Clone(identificador, null, null, null, false);
    }

    public IAuditContext Clone(string identificador)
    {
        return Clone(identificador, null, null, null, false);
    }

    public IAuditContext Clone(EnumAcao acao)
    {
        return Clone(null, null, null, acao, false);
    }

    public IAuditContext Clone(string identificador, EnumAcao acao)
    {
        return Clone(identificador, null, null, acao, false);
    }

    public IAuditContext Clone(EnumSecao secao)
    {
        return Clone(null, null, secao, null, false);
    }

    public IAuditContext Clone(EnumSecao secao, EnumAcao acao)
    {
        return Clone(null, null, secao, acao, false);
    }

    public IAuditContext Clone(string identificador, EnumSecao secao, EnumAcao acao)
    {
        return Clone(identificador, null, secao, acao, false);
    }

    public IAuditContext Clone(string identificador, EnumFuncionalidade? funcionalidade, EnumSecao? secao, EnumAcao? acao)
    {
        return Clone(identificador, funcionalidade, secao, acao, false);
    }

    public IAuditContext Clone(string identificador, EnumFuncionalidade? funcionalidade, EnumSecao? secao, EnumAcao? acao, bool atualizarSecao)
    {
        var obj = (IAuditContext)this.MemberwiseClone();
        obj.Identificador = identificador;
        if (funcionalidade.HasValue)
        {
            obj.Funcionalidade = funcionalidade.Value;
        }

        if (secao.HasValue || atualizarSecao)
        {
            obj.Secao = secao;
        }

        if (acao.HasValue)
        {
            obj.Acao = acao.Value;
        }

        return obj;
    }

    #endregion
}