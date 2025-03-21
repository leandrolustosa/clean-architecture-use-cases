public class ProcessoOfertaNegociacaoExcluirValidation : EntityValidation<ProcessoOfertaNegociacao, ProcessoOfertaNegociacaoExcluirDto>, IProcessoOfertaNegociacaoExcluirValidation        
{
    private readonly IProcessoOfertaRepository repositoryOferta;
    public ProcessoOfertaNegociacaoExcluirValidation(IProcessoOfertaRepository repositoryOferta,
        IRepository<ProcessoOfertaNegociacao> repository)
        : base(repository)
    {
        this.repositoryOferta = repositoryOferta;
    }

    public override async Task<ISingleResult<ProcessoOfertaNegociacao>> ValidarAsync(ProcessoOfertaNegociacao entity)
    {
        var registroExiste = await RegistroExisteAsync(entity.Id, nameof(ProcessoOfertaNegociacao.HistoricoOfertaNegociacao));
        if (!registroExiste.Sucesso)
        {
            return registroExiste;
        }

        var oferta = await repositoryOferta.GetOfertaNegociacaoAsync(registroExiste.Data.IdProcessoOferta);

        var possuiOfertaOnSubs = await repositoryOferta.PossuiOfertaOnSubsPorAberturaAsync(oferta.IdProcessoAbertura);
        if (possuiOfertaOnSubs)
        {
            return new SingleResult<ProcessoOfertaNegociacao>(MensagensNegocio.MSG41);
        }

        var ultimaNegociacao = oferta.ProcessoOfertaNegociacao.LastOrDefault();

        if (ultimaNegociacao.Id != entity.Id)
        {
            return new SingleResult<ProcessoOfertaNegociacao>(MensagensNegocio.MSG40);
        }

        if (entity.OrigemOferta == "I")
        {
            return new SingleResult<ProcessoOfertaNegociacao>(MensagensNegocio.MSG47);
        }

        return await Task.Run(() => new SingleResult<ProcessoOfertaNegociacao>(registroExiste.Data));
    }
}