public interface IProcessoOfertaNegociacaoRepository : IRepository<ProcessoOfertaNegociacao>
{
    IQueryable<ProcessoOfertaNegociacao> GetNegociacoesPorOfertaAsync();
}
