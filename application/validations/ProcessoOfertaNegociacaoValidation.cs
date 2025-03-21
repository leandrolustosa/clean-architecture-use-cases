public class ProcessoOfertaNegociacaoValidation<TDto> : DtoValidation<TDto>
    where TDto : ProcessoOfertaNegociacaoDto
{
    protected void ValidarNumeroRodada()
    {
        RuleFor(v => v.NumeroRodada)                
            .LessThan(10000).WithMessage(MensagensAplicacao.CAMPO_MENOR_QUE)
            .WithName("Rodada");
    }

    protected void ValidarOrigemOferta()
    {
        RuleFor(v => v.OrigemOferta)
            .NotNull().WithMessage(MensagensAplicacao.CAMPO_OBRIGATORIO)
            .MaximumLength(1).WithMessage(MensagensAplicacao.TAMANHO_ESPECIFICO_CAMPO)
            .WithName("Origem da Oferta");
    }

    protected void ValidarProcessoOfertaId()
    {
        RuleFor(v => v.IdProcessoOferta)
            .NotEqual(0).WithMessage(MensagensAplicacao.CAMPO_OBRIGATORIO)                
            .WithName("Identificador da Oferta");
    }

    protected void ValidarValorTaxaFrete()
    {
        RuleFor(v => v.ValorTaxaFrete)
            .NotNull().WithMessage(MensagensAplicacao.CAMPO_OBRIGATORIO)
            .NotEqual(0).WithMessage(MensagensAplicacao.CAMPO_OBRIGATORIO)
            .WithName("Taxa de Frete");
    }

    protected void ValidarValorTaxaSobreEstadia()
    {
        RuleFor(v => v.ValorTaxaSobreEstadia)
            .NotNull().WithMessage(MensagensAplicacao.CAMPO_OBRIGATORIO)
            .NotEqual(0).WithMessage(MensagensAplicacao.CAMPO_OBRIGATORIO)
            .WithName("Taxa de Sobre-estadia");
    }

    protected void ValidarQuantidadeLote()
    {
        RuleFor(v => v.QuantidadeLote)
            .NotNull().WithMessage(MensagensAplicacao.CAMPO_OBRIGATORIO)
            .NotEqual(0).WithMessage(MensagensAplicacao.CAMPO_OBRIGATORIO)
            .WithName("Lote (T)");
    }

    protected void ValidarQuantidadeTempoLaytime()
    {
        RuleFor(v => v.QuantidadeTempoLaytime)
            .Must((model, field) => model.UnidadeMedidaSobreEstadia == null
            || model.UnidadeMedidaSobreEstadia == EnumUnidadeMedidaSobreEstadia.ToneladaPorHora
            || (model.UnidadeMedidaSobreEstadia == EnumUnidadeMedidaSobreEstadia.Hora && field.GetValueOrDefault(0) != 0))
            .WithName("Laytime");
    }

    protected void ValidarQuantidadeVazaoCarga()
    {
        RuleFor(v => v.QuantidadeVazaoCarga)
            .Must((model, field) => model.UnidadeMedidaSobreEstadia == null 
            || model.UnidadeMedidaSobreEstadia == EnumUnidadeMedidaSobreEstadia.Hora 
            || (model.UnidadeMedidaSobreEstadia == EnumUnidadeMedidaSobreEstadia.ToneladaPorHora && field.GetValueOrDefault(0) != 0))
            .WithName("Vazão Carga");
    }

    protected void ValidarQuantidadeVazaoDescarga()
    {
        RuleFor(v => v.QuantidadeVazaoDescarga)
            .Must((model, field) => model.UnidadeMedidaSobreEstadia == null
            || model.UnidadeMedidaSobreEstadia == EnumUnidadeMedidaSobreEstadia.Hora
            || (model.UnidadeMedidaSobreEstadia == EnumUnidadeMedidaSobreEstadia.ToneladaPorHora && field.GetValueOrDefault(0) != 0))
            .WithName("Vazão Descarga");
    }
}