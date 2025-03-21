[AuditableEntity(Acao = Enums.EnumAcao.Inclusao, Funcionalidade = Enums.EnumFuncionalidade.ProcessoAfretamento, Secao = Enums.EnumSecao.Oferta)]
public class ProcessoOfertaNegociacaoDto : AuditableEntityDto
{
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
    public long IdProcessoOferta { get; set; }
    public EnumUnidadeMedidaSobreEstadia? UnidadeMedidaSobreEstadia { get; set; }
    public EnumUnidadeMedidaFrete? UnidadeMedidaFrete { get; set; }
}