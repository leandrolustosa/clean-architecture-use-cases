public partial class ProcessoOfertaNegociacao : AuditableEntity
{
    public ProcessoOfertaNegociacao()
    {
        HistoricoOfertaNegociacao = new HashSet<ProcessoOfertaNegociacaoHistorico>();
    }
    public override string Value => Id.ToString();

    public long NumeroRodada { get; set; }
    public string OrigemOferta { get; set; }
    public decimal? ValorTaxaFrete { get; set; }
    public decimal? ValorTaxaSobreEstadia { get; set; }
    public decimal? QuantidadeTempoLaytime { get; set; }
    public decimal? QuantidadeLote { get; set; }
    public decimal? ValorFinal { get; set; }
    public decimal? ValorDolarTonelada { get; set; }
    public bool IndicadorAtivo { get; set; }
    public decimal? QuantidadeVazaoCarga { get; set; }
    public decimal? QuantidadeVazaoDescarga { get; set; }

    [DefaultValue(nameof(ProcessoOferta))]
    public long IdProcessoOferta { get; set; }
    public virtual ProcessoOferta ProcessoOferta { get; set; }

    public virtual ICollection<ProcessoOfertaNegociacaoHistorico> HistoricoOfertaNegociacao { get; set; }
}