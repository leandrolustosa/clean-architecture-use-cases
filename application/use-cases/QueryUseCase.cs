public abstract class QueryUseCase<TLoadOptions> : IQueryUseCase<TLoadOptions> where TLoadOptions : DataSourceLoadOptionsBase
{
    protected readonly IMapper mapper;

    protected readonly IDevExtremeManager devExtremeManager;

    protected readonly TLoadOptions loadOptions;

    protected QueryUseCase(TLoadOptions loadOptions, IMapper mapper, IDevExtremeManager devExtremeManager)
    {
        this.loadOptions = loadOptions;
        this.mapper = mapper;
        this.devExtremeManager = devExtremeManager;
    }

    public abstract Task<ILoadResultDto> ExecuteAsync();
}