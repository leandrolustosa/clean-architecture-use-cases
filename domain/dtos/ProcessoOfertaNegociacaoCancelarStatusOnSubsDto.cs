[AuditableEntity(Acao = Enums.EnumAcao.CancelamentoOnSubs, Funcionalidade = Enums.EnumFuncionalidade.ProcessoAfretamento, Secao = Enums.EnumSecao.Negociacao)]
public class ProcessoOfertaNegociacaoCancelarStatusOnSubsDto : AuditableEntityDto
{
    public long IdTipoSituacaoOferta { get { return (long)EnumSituacaoProcessoOferta.EmNegociacao; } }
}