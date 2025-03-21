public class ProcessoOfertaNegociacaoListarUseCase : QueryUseCase<DataSourceLoadOptions>, IProcessoOfertaNegociacaoListarUseCase
{
    private readonly IProcessoOfertaNegociacaoRepository _repository;
    public ProcessoOfertaNegociacaoListarUseCase(DataSourceLoadOptions loadOptions, IMapper mapper, IDevExtremeManager devExtremeManager,
        IProcessoOfertaNegociacaoRepository repository) : base(loadOptions, mapper, devExtremeManager)
    {
        _repository = repository;
    }

    public override async Task<ILoadResultDto> ExecuteAsync()
    {
        var query = _repository.GetNegociacoesPorOfertaAsync();            
        var result = await devExtremeManager.GetResultAsync(query, loadOptions);
        return new LoadResultDto(result);
    }
}
