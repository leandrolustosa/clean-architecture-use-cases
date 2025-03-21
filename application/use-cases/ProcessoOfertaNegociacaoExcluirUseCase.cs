public class ProcessoOfertaNegociacaoExcluirUseCase : AuditableUseCase<ProcessoOfertaNegociacaoExcluirDto>, IProcessoOfertaNegociacaoExcluirUseCase
{   
    private readonly IRepository<ProcessoOfertaNegociacao> _repository;
    private readonly IRepository<ProcessoOferta> _repositoryOferta;
    private readonly IRepository<ProcessoOfertaNegociacaoHistorico> _repositoryHistorico;
    private readonly IEntityValidation<ProcessoOfertaNegociacao, ProcessoOfertaNegociacaoExcluirDto> _validation;        

    public ProcessoOfertaNegociacaoExcluirUseCase(ProcessoOfertaNegociacaoExcluirDto dto, IRepository<ProcessoOfertaNegociacao> repository,
        IRepository<ProcessoOferta> repositoryOferta, IRepository<ProcessoOfertaNegociacaoHistorico> repositoryHistorico, 
        IEntityValidation<ProcessoOfertaNegociacao, ProcessoOfertaNegociacaoExcluirDto> validation, 
        IAuditContext auditContext, User user, IMapper mapper, IUnitOfWork uow) 
        : base(dto, user, auditContext, mapper, uow)
    {            
        _repository = repository;
        _repositoryOferta = repositoryOferta;
        _repositoryHistorico = repositoryHistorico;
        _validation = validation;
    }

    public override async Task<ISingleResultDto<ProcessoOfertaNegociacaoExcluirDto>> ExecuteAsync()
    {
        var validationResult = await _validation.ValidarAsync(new ProcessoOfertaNegociacao { Id = dto.Id });
        if (!validationResult.Sucesso)
        {
            return new SingleResultDto<ProcessoOfertaNegociacaoExcluirDto>(validationResult);
        }

        var negociacao = validationResult.Data;

        var oferta = await _repositoryOferta.GetByIdAsync(negociacao.IdProcessoOferta,
            nameof(ProcessoOferta.ProcessoOfertaNegociacao),
            $"{nameof(ProcessoOferta.ProcessoAbertura)}.{nameof(ProcessoOferta.ProcessoAbertura.ProcessoAfretamento)}");

        oferta.ProcessoOfertaNegociacao.Remove(negociacao);

        var negociacoes = oferta.ProcessoOfertaNegociacao.OrderBy(p => p.NumeroRodada).ThenByDescending(p => p.OrigemOferta).ToList();
        var ultimaNegociacao = negociacoes.LastOrDefault(p => p.OrigemOferta == "C");
        if (ultimaNegociacao == null)
        {
            ultimaNegociacao = negociacoes.FirstOrDefault(p => p.OrigemOferta == "I");
        }

        ultimaNegociacao.IndicadorAtivo = true;

        _repository.Update(ultimaNegociacao);

        foreach (var historico in negociacao.HistoricoOfertaNegociacao)
        {
            _repositoryHistorico.Remove(historico);
        }

        _repository.Remove(negociacao);

        await CommitAsync();

        return new SingleResultDto<ProcessoOfertaNegociacaoExcluirDto>(dto);
    }
}