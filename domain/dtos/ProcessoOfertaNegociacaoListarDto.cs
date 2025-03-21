public class ProcessoOfertaNegociacaoListarDto : EntityDto
{
    public long NumeroRodada { get; set; }
    public string OrigemOferta { get; set; }
    public decimal? ValorTaxaFrete { get; set; }
    public decimal? ValorTaxaSobreEstadia { get; set; }
    public decimal? QuantidadeTempoLaytime { get; set; }
    public decimal? QuantidadeLote { get; set; }
    public decimal? ValorFinal { get; set; }
    public decimal? ValorDolarTonelada { get; set; }
    public long IdProcessoAbertura { get; set; }
}
