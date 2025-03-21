public class ProcessoOfertaNegociacaoEditarValidation : ProcessoOfertaNegociacaoValidation<ProcessoOfertaNegociacaoEditarDto>
{
    public ProcessoOfertaNegociacaoEditarValidation()
    {
        ValidarId();
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

    protected void ValidarTextoJustificativa()
    {
        RuleFor(v => v.TextoJustificativa)
            .NotNull().WithMessage(MensagensAplicacao.CAMPO_OBRIGATORIO)
            .MaximumLength(2000).WithMessage(MensagensAplicacao.TAMANHO_ESPECIFICO_CAMPO)
            .WithName("Justificativa");
    }
}