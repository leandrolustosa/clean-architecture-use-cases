public class LookupUseCase<TEntity> : ILookupUseCase<TEntity>
    where TEntity : Entity
{
    protected readonly IMapper Mapper;
    private readonly IRepository<TEntity> _repository;
    private readonly IDevExtremeManager _devExtremeManager;
    private readonly LookupDataSourceLoadOptions _loadOptions;

    public LookupUseCase(LookupDataSourceLoadOptions loadOptions, IMapper mapper, IRepository<TEntity> repository, IDevExtremeManager devExtremeManager)
    {
        _repository = repository;
        _devExtremeManager = devExtremeManager;
        _loadOptions = loadOptions;
        Mapper = mapper;
    }

    public Task<IEnumerable<LookupDto>> ExecuteAsync()
    {
        IQueryable<LookupEntity> lookup = _repository.GetLookup();
        return _devExtremeManager.GetResultAsync(lookup, _loadOptions);
    }
}