public class ProcessoOfertaNegociacaoIncluirUseCase : AuditableUseCase<ProcessoOfertaNegociacaoIncluirDto>, IProcessoOfertaNegociacaoIncluirUseCase
{        
    private readonly IProcessoOfertaRepository _ofertaRepository;        
    private readonly IProcessoOfertaNegociacaoIncluirValidation _validation;
    private readonly IUseCaseManager _manager;

    public ProcessoOfertaNegociacaoIncluirUseCase(ProcessoOfertaNegociacaoIncluirDto dto, IProcessoOfertaRepository ofertaRepository, 
        IProcessoOfertaNegociacaoIncluirValidation validation, IUseCaseManager manager, 
        IMapper mapper, IAuditContext auditContext, User user, IUnitOfWork uow) 
        : base(dto, user, auditContext, mapper, uow)
    {
        _ofertaRepository = ofertaRepository;            
        _validation = validation;
        _manager = manager;
    }

    public override async Task<ISingleResultDto<ProcessoOfertaNegociacaoIncluirDto>> ExecuteAsync()
    {
        var negociacao = mapper.Map<ProcessoOfertaNegociacao>(dto);

        var resultValidation = await _validation.ValidarAsync(negociacao);
        if (!resultValidation.Sucesso)
        {
            return new SingleResultDto<ProcessoOfertaNegociacaoIncluirDto>(resultValidation);
        }

        var oferta = await _ofertaRepository.GetByIdAsync(dto.IdProcessoOferta, 
            nameof(ProcessoOferta.ProcessoOfertaNegociacao), 
            $"{nameof(ProcessoOferta.ProcessoAbertura)}.{nameof(ProcessoOferta.ProcessoAbertura.ProcessoAfretamento)}");

        negociacao.IndicadorAtivo = false;
        if (negociacao.OrigemOferta == "C")
        {
            foreach(var item in oferta.ProcessoOfertaNegociacao)
            {
                item.IndicadorAtivo = false;
            }

            negociacao.IndicadorAtivo = true;
        }

        var ofertaDto = mapper.Map<ProcessoOfertaCalculoDto>(oferta);

        var calculo = new CalculoValorOfertaNegociacaoDto { Id = oferta.IdProcessoAbertura, Oferta = ofertaDto, Negociacao = dto };

        var resultCalculo = await _manager.ResolveUseCase<ICalcularValorOfertaNegociacaoUseCase, CalculoValorOfertaNegociacaoDto>(calculo).ExecuteAsync();
        if (resultCalculo.Sucesso)
        {
            negociacao.ValorDolarTonelada = resultCalculo.Data.ValorDolarTonelada;
            negociacao.ValorFinal = resultCalculo.Data.ValorFinal;
        }

        negociacao.AuditContext.Identificador = oferta.ProcessoAbertura.ProcessoAfretamento.CodigoProcesso;

        oferta.ProcessoOfertaNegociacao.Add(negociacao);

        _ofertaRepository.Update(oferta);

        await CommitAsync();

        return new SingleResultDto<ProcessoOfertaNegociacaoIncluirDto>(dto);
    }
}