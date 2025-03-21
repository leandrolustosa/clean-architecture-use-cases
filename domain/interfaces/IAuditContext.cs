public interface IAuditContext
{
    string Identificador { get; set; }
    EnumAcao Acao { get; set; }
    EnumFuncionalidade Funcionalidade { get; set; }
    EnumSecao? Secao { get; set; }
    string TextoJustificativa { get; set; }

    IAuditContext Clone();
    IAuditContext Clone(string identificador);
    IAuditContext Clone(EnumAcao acao);
    IAuditContext Clone(string identificador, EnumAcao acao);
    IAuditContext Clone(EnumSecao secao);
    IAuditContext Clone(EnumSecao secao, EnumAcao acao);
    IAuditContext Clone(string identificador, EnumSecao secao, EnumAcao acao);
    IAuditContext Clone(string identificador, EnumFuncionalidade? funcionalidade, EnumSecao? secao, EnumAcao? acao);
    IAuditContext Clone(string identificador, EnumFuncionalidade? funcionalidade, EnumSecao? secao, EnumAcao? acao, bool atualizarSecao);
}