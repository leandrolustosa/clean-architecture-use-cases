public class ProcessoOfertaNegociacaoIncluirValidation : EntityValidation<ProcessoOfertaNegociacao, ProcessoOfertaNegociacaoIncluirDto>, IProcessoOfertaNegociacaoIncluirValidation
{
    private readonly IProcessoOfertaRepository repositoryOferta;
    public ProcessoOfertaNegociacaoIncluirValidation(IProcessoOfertaRepository repositoryOferta,
        IRepository<ProcessoOfertaNegociacao> repository)
        : base(repository)
    {
        this.repositoryOferta = repositoryOferta;
    }

    public override async Task<ISingleResult<ProcessoOfertaNegociacao>> ValidarAsync(ProcessoOfertaNegociacao entity)
    {
        var oferta = await repositoryOferta.GetOfertaNegociacaoAsync(entity.IdProcessoOferta);

        var possuiOfertaOnSubs = await repositoryOferta.PossuiOfertaOnSubsPorAberturaAsync(oferta.IdProcessoAbertura);
        if (possuiOfertaOnSubs)
        {
            return new SingleResult<ProcessoOfertaNegociacao>(MensagensNegocio.MSG41);
        }

        return await Task.Run(() => new SingleResult<ProcessoOfertaNegociacao>());
    }
}