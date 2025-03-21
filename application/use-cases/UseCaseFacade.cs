public class UseCaseFacade : IUseCaseFacade
{
    private readonly IUseCaseManager manager;
    private readonly ILogger<UseCaseFacade> logger;

    public UseCaseFacade(IUseCaseManager manager, ILogger<UseCaseFacade> logger)
    {
        this.manager = manager;
        this.logger = logger;
    }

    public async Task<ISingleResultDto<TDto>> ExecuteAsync<TUseCase, TDto>(TDto dto)
        where TUseCase : IUseCase<TDto>
        where TDto : EntityDto
    {
        try
        {
            var useCase = manager.ResolveUseCase<TUseCase, TDto>(dto);
            
            return await useCase.ExecuteAsync();
        }
        catch (Exception ex)
        {
            logger.LogError((ex.InnerException != null) ? ex.InnerException.ToString() : ex.ToString());

            return new SingleResultDto<TDto>(ex);
        }
    }

    public async Task<ISingleResultDto<TDtoReturn>> ExecuteAsync<TUseCase, TDto, TDtoReturn>(TDto dto)
        where TUseCase : IUseCase<TDto, TDtoReturn>
        where TDto : EntityDto
        where TDtoReturn : EntityDto
    {
        try
        {
            var useCase = manager.ResolveUseCase<TUseCase, TDto, TDtoReturn>(dto);

            return await useCase.ExecuteAsync();
        }
        catch (Exception ex)
        {
            logger.LogError((ex.InnerException != null) ? ex.InnerException.ToString() : ex.ToString());

            return new SingleResultDto<TDtoReturn>(ex);
        }
    }

    public async Task<ISingleResultDto<TDto>> ExecuteAuditableAsync<TUseCase, TDto>(TDto dto, User user)
        where TUseCase : IAuditableUseCase<TDto>
        where TDto : AuditableEntityDto
    {
        try
        {
            var useCase = manager.ResolveAuditableUseCase<TUseCase, TDto>(dto, user);
            manager.Prepare(useCase, dto, user);

            return await useCase.ExecuteAsync();
        }
        catch (Exception ex)
        {
            logger.LogError((ex.InnerException != null) ? ex.InnerException.ToString() : ex.ToString());

            return new SingleResultDto<TDto>(ex);
        }
    }

    public async Task<ISingleResultDto<TDtoReturn>> ExecuteAuditableAsync<TUseCase, TDto, TDtoReturn>(TDto dto, User user)
        where TUseCase : IAuditableUseCase<TDto, TDtoReturn>
        where TDto : AuditableEntityDto
        where TDtoReturn : EntityDto
    {
        try
        {
            var useCase = manager.ResolveAuditableUseCase<TUseCase, TDto, TDtoReturn>(dto, user);
            manager.Prepare(useCase, dto, user);

            return await useCase.ExecuteAsync();
        }
        catch (TimeoutException ex)
        {
            logger.LogError((ex.InnerException != null) ? ex.InnerException.ToString() : ex.ToString());

            return new SingleResultDto<TDtoReturn>(ex);
        }
        catch (Exception ex)
        {
            logger.LogError((ex.InnerException != null) ? ex.InnerException.ToString() : ex.ToString());

            return new SingleResultDto<TDtoReturn>(ex);
        }
    }

    public async Task<ILoadResultDto> QueryAsync<TUseCase, TLoadOptions>(TLoadOptions loadOptions)
        where TUseCase : IQueryUseCase<TLoadOptions>
        where TLoadOptions : DataSourceLoadOptionsBase
    {
        try
        {
            var useCase = manager.ResolveQueryUseCase<TUseCase, TLoadOptions>(loadOptions);
            return await useCase.ExecuteAsync();
        }
        catch (Exception ex)
        {
            return new LoadResultDto(ex);
        }
    }

    public async Task<ListResultDto<LookupDto>> LookupAsync<TEntity>(LookupDataSourceLoadOptions loadOptions)
        where TEntity : Entity
    {
        try
        {
            var useCase = manager.ResolveLookupUseCase<TEntity>(loadOptions);
            var list = await useCase.ExecuteAsync();
            return new ListResultDto<LookupDto>(list);
        }
        catch (Exception ex)
        {
            return new ListResultDto<LookupDto>(ex);
        }
    }

    public async Task<ListResultDto<TLookupDto>> LookupQueryAsync<TLookupDto, TEntity, TLoadOptions>(TLoadOptions loadOptions)
        where TLookupDto : EntityDto<long>
        where TEntity : Entity
        where TLoadOptions : DataSourceLoadOptionsBase
    {
        try
        {
            var useCase = manager.ResolveLookupQueryUseCase<TLookupDto, TEntity>(loadOptions);
            var list = await useCase.ExecuteAsync();
            return new ListResultDto<TLookupDto>(list);
        }
        catch (Exception ex)
        {
            return new ListResultDto<TLookupDto>();
        }
    }
}