public class ProcessoOfertaNegociacaoRepository : Repository<ProcessoOfertaNegociacao>, IProcessoOfertaNegociacaoRepository
{
    public ProcessoOfertaNegociacaoRepository(AplicationDbContext context) : base(context)
    {
    }

    public IQueryable<ProcessoOfertaNegociacao> GetNegociacoesPorOfertaAsync()
    {
        return DbSet                
            .OrderBy(o => o.NumeroRodada)
            .ThenByDescending(o => o.OrigemOferta)
            .Select(negociacao => new ProcessoOfertaNegociacao
            {
                Id = negociacao.Id,
                IdProcessoOferta = negociacao.IdProcessoOferta,
                NumeroRodada = negociacao.NumeroRodada,
                OrigemOferta = negociacao.OrigemOferta,
                IndicadorAtivo = negociacao.IndicadorAtivo,
                QuantidadeVazaoCarga = negociacao.QuantidadeVazaoCarga,
                QuantidadeVazaoDescarga = negociacao.QuantidadeVazaoDescarga,
                QuantidadeLote = negociacao.QuantidadeLote,
                QuantidadeTempoLaytime = negociacao.QuantidadeTempoLaytime,
                ValorDolarTonelada = negociacao.ValorDolarTonelada,
                ValorFinal = negociacao.ValorFinal,
                ValorTaxaFrete = negociacao.ValorTaxaFrete,
                ValorTaxaSobreEstadia = negociacao.ValorTaxaSobreEstadia
            });
    }
}