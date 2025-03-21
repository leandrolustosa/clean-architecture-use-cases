public interface IQueryUseCase<TLoadOptions>
    where TLoadOptions : DataSourceLoadOptionsBase
{
    Task<ILoadResultDto> ExecuteAsync();
}
