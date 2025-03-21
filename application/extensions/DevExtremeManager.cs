public class DevExtremeManager : IDevExtremeManager
{
    public Task<LoadResult> GetResultAsync<TEntity>(IQueryable<TEntity> query, DataSourceLoadOptionsBase options)
    {
        return DataSourceLoader.LoadAsync(query, options);
    }

    public async Task<IEnumerable<LookupDto>> GetResultAsync(IQueryable<LookupEntity> query, LookupDataSourceLoadOptions options)
    {
        var loadResult = await DataSourceLoader.LoadAsync(query, options);
        return loadResult.data.Cast<ILookupEntity<long>>() 
            .Select(s => new LookupDto { Key = s.Key, Value = s.Value })
            .OrderBy(o => o.Value);
    }

    public async Task<IEnumerable<LookupEntityStringDto>> GetResultAsync(IQueryable<LookupStringEntity> query, LookupDataSourceLoadOptions options)
    {
        var loadResult = await DataSourceLoader.LoadAsync(query, options);
        return loadResult.data.Cast<ILookupEntity<string>>()
            .Select(s => new LookupEntityStringDto { Key = s.Key, Value = s.Value })
            .OrderBy(o => o.Value);
    }
}
