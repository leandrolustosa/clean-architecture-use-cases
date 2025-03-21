public class LookupQueryUseCase<TLookupDto, TEntity> : ILookupQueryUseCase<TLookupDto, TEntity>
    where TLookupDto : EntityDto<long>
    where TEntity : Entity
{
    protected readonly IMapper Mapper;
    
    private readonly ILookupQueryRepository<TEntity> _repository;
    private readonly IDevExtremeManager _devExtremeManager;
    private readonly DataSourceLoadOptionsBase _loadOptions;

    public LookupQueryUseCase(IMapper mapper, ILookupQueryRepository<TEntity> repository, IDevExtremeManager devExtremeManager, DataSourceLoadOptionsBase loadOptions)
    {
        _repository = repository;
        _devExtremeManager = devExtremeManager;
        _loadOptions = loadOptions;
        Mapper = mapper;
    }

    public async Task<IEnumerable<TLookupDto>> ExecuteAsync()
    {
        IQueryable<TEntity> lookup = _repository.GetLookupQuery();

        var source = lookup.ProjectTo<TLookupDto>(Mapper.ConfigurationProvider);

        var list = await _devExtremeManager.GetResultAsync(source, _loadOptions);

        var result = list.data.OfType<TLookupDto>().ToList();

        return result;
    }
}