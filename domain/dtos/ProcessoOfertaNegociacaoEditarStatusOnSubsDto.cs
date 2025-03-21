[AuditableEntity(Acao = Enums.EnumAcao.Alteracao, Funcionalidade = Enums.EnumFuncionalidade.ProcessoAfretamento, Secao = Enums.EnumSecao.Negociacao)]
public class ProcessoOfertaNegociacaoEditarStatusOnSubsDto : ProcessoOfertaNegociacaoEditarStatusDto
{
    public override long IdTipoSituacaoOferta { get { return (long)EnumSituacaoProcessoOferta.OnSubs; } }
}