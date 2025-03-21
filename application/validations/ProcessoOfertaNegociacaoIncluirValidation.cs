public class ProcessoOfertaNegociacaoIncluirValidation : ProcessoOfertaNegociacaoValidation<ProcessoOfertaNegociacaoIncluirDto>
{
    public ProcessoOfertaNegociacaoIncluirValidation()
    {
        ValidarProcessoOfertaId();
        ValidarNumeroRodada();
        ValidarOrigemOferta();
        ValidarQuantidadeLote();
        ValidarQuantidadeTempoLaytime();
        ValidarQuantidadeVazaoCarga();
        ValidarQuantidadeVazaoDescarga();
        ValidarValorTaxaFrete();
        ValidarValorTaxaSobreEstadia();
    }
}