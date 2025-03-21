[AuditableEntity(Acao = EnumAcao.Alteracao, Funcionalidade = EnumFuncionalidade.ProcessoAfretamento, Secao = EnumSecao.Negociacao)]
public class ProcessoOfertaNegociacaoEditarDto : ProcessoOfertaNegociacaoDto
{
    public string CodigoProcesso { get; set; }
    public string TextoJustificativa { get; set; }
}