public class ProcessoOfertaNegociacaoCancelarStatusOnSubsValidation : EntityValidation<Domain.Processos.ProcessoOferta, ProcessoOfertaNegociacaoCancelarStatusOnSubsDto>, IProcessoOfertaNegociacaoCancelarStatusOnSubsValidation
{        
    public ProcessoOfertaNegociacaoCancelarStatusOnSubsValidation(IRepository<Domain.Processos.ProcessoOferta> repository)
        : base(repository)
    {            
    }

    public override async Task<ISingleResult<Domain.Processos.ProcessoOferta>> ValidarAsync(Domain.Processos.ProcessoOferta entity, ProcessoOfertaNegociacaoCancelarStatusOnSubsDto dto)
    {
        var situacaoAtual = (EnumSituacaoProcessoOferta)entity.IdTipoSituacaoOferta;            

        if (situacaoAtual != EnumSituacaoProcessoOferta.OnSubs)
        {
            return new SingleResult<Domain.Processos.ProcessoOferta>(MensagensNegocio.MSG48);
        }

        if (entity.ProcessoAbertura.DataContrato.HasValue)
        {
            return new SingleResult<Domain.Processos.ProcessoOferta>(MensagensNegocio.MSG50);
        }

        return await Task.Run(() => new SingleResult<Domain.Processos.ProcessoOferta>());
    }
}