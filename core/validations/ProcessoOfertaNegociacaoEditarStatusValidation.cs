public class ProcessoOfertaNegociacaoEditarStatusValidation<TDto> : EntityValidation<Domain.Processos.ProcessoOferta, TDto>, IProcessoOfertaNegociacaoEditarStatusValidation<TDto>
    where TDto : ProcessoOfertaNegociacaoEditarStatusDto
{   
    public ProcessoOfertaNegociacaoEditarStatusValidation(IRepository<Domain.Processos.ProcessoOferta> repository)
        : base(repository)
    {            
    }

    public override async Task<ISingleResult<Domain.Processos.ProcessoOferta>> ValidarAsync(Domain.Processos.ProcessoOferta entity, TDto dto)
    {
        var situacaoAtual = (EnumSituacaoProcessoOferta)entity.IdTipoSituacaoOferta;
        var situacaoFinal = (EnumSituacaoProcessoOferta)dto.IdTipoSituacaoOferta;
        
        if (situacaoAtual == EnumSituacaoProcessoOferta.EmNegociacao && situacaoFinal != EnumSituacaoProcessoOferta.NegociacaoEncerradaPelaContraparte && situacaoFinal != EnumSituacaoProcessoOferta.OnSubs)
        {
            return new SingleResult<Domain.Processos.ProcessoOferta>(MensagensNegocio.MSG22);
        }

        return await Task.Run(() => new SingleResult<Domain.Processos.ProcessoOferta>());
    }
}