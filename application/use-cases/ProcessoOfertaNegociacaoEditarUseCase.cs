public class ProcessoOfertaNegociacaoEditarUseCase : AuditableUseCase<ProcessoOfertaNegociacaoEditarDto>, IProcessoOfertaNegociacaoEditarUseCase
{   
    private readonly IRepository<ProcessoOfertaNegociacao> _repository;
    private readonly IEntityValidation<ProcessoOfertaNegociacao, ProcessoOfertaNegociacaoEditarDto> _validation;
    private readonly IUseCaseManager _manager;

    public ProcessoOfertaNegociacaoEditarUseCase(ProcessoOfertaNegociacaoEditarDto dto, 
        IRepository<ProcessoOfertaNegociacao> repository, IEntityValidation<ProcessoOfertaNegociacao, ProcessoOfertaNegociacaoEditarDto> validation, 
        IAuditContext auditContext, User user, IMapper mapper, IUnitOfWork uow, IUseCaseManager manager) 
        : base(dto, user, auditContext, mapper, uow)
    {            
        _repository = repository;            
        _validation = validation;
        _manager = manager;
    }

    public override async Task<ISingleResultDto<ProcessoOfertaNegociacaoEditarDto>> ExecuteAsync()
    {
        var registroResult = await _validation.RegistroExisteAsync(dto.Id, nameof(ProcessoOfertaNegociacao.ProcessoOferta));
        if (!registroResult.Sucesso)
        {
            return new SingleResultDto<ProcessoOfertaNegociacaoEditarDto>(registroResult);
        }

        var negociacao = mapper.Map(dto, registroResult.Data);

        var ofertaDto = mapper.Map<ProcessoOfertaCalculoDto>(registroResult.Data.ProcessoOferta);

        var calculo = new CalculoValorOfertaNegociacaoDto { Id = ofertaDto.IdProcessoAbertura, Oferta = ofertaDto, Negociacao = dto };

        var resultCalculo = await _manager.ResolveUseCase<ICalcularValorOfertaNegociacaoUseCase, CalculoValorOfertaNegociacaoDto>(calculo).ExecuteAsync();
        if (resultCalculo.Sucesso)
        {
            negociacao.ValorDolarTonelada = resultCalculo.Data.ValorDolarTonelada;
            negociacao.ValorFinal = resultCalculo.Data.ValorFinal;
        }

        if (!string.IsNullOrEmpty(dto.TextoJustificativa))
        {
            var historico = new ProcessoOfertaNegociacaoHistorico
            {
                TextoJustificativa = dto.TextoJustificativa,
                ChaveUsuarioAcao = negociacao.User.Chave,
                DataAcao = DateTime.Now
            };

            ConfigureAuditContext(historico, dto.CodigoProcesso, true, EnumAcao.Inclusao);

            negociacao.HistoricoOfertaNegociacao.Add(historico);
        }

        _repository.Update(negociacao);

        await CommitAsync();

        return new SingleResultDto<ProcessoOfertaNegociacaoEditarDto>(dto);
    }
}