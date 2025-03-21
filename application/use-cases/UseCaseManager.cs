public class UseCaseManager : IUseCaseManager
{
    private readonly IComponentContext container;

    public UseCaseManager(IComponentContext container)
    {
        this.container = container;
    }

    public void Prepare<TDto>(IAuditableUseCase<TDto> useCase, TDto dto, User user)
        where TDto : AuditableEntityDto
    {
        var attr = typeof(TDto).GetCustomAttributes(false).OfType<AuditableEntityAttribute>().FirstOrDefault();

        if (attr != null && useCase.AuditContext != null)
        {
            useCase.AuditContext.Funcionalidade = attr.Funcionalidade;
            useCase.AuditContext.Secao = (attr.Secao != Domain.Enums.EnumSecao.NaoInformada) ? attr.Secao : (Domain.Enums.EnumSecao?)null;
            useCase.AuditContext.Acao = attr.Acao;
            useCase.AuditContext.TextoJustificativa = dto.TextoJustificativaAuditable;
            dto.AuditContext = (AuditContext)useCase.AuditContext;
            dto.User = user;
        }
        else
        {
            useCase.AuditContext = null;
        }
    }

    public void Prepare<TDto, TDtoReturn>(IAuditableUseCase<TDto, TDtoReturn> useCase, TDto dto, User user)
        where TDto : AuditableEntityDto
        where TDtoReturn : EntityDto
    {
        var attr = typeof(TDto).GetCustomAttributes(false).OfType<AuditableEntityAttribute>().FirstOrDefault();

        if (attr != null && useCase.AuditContext != null)
        {
            useCase.AuditContext.Funcionalidade = attr.Funcionalidade;
            useCase.AuditContext.Secao = (attr.Secao != Domain.Enums.EnumSecao.NaoInformada) ? attr.Secao : (Domain.Enums.EnumSecao?)null;
            useCase.AuditContext.Acao = attr.Acao;
            useCase.AuditContext.TextoJustificativa = dto.TextoJustificativaAuditable;
            dto.AuditContext = (AuditContext)useCase.AuditContext;
            dto.User = user;
        }
        else
        {
            useCase.AuditContext = null;
        }
    }

    public IUseCase<TDto> ResolveUseCase<TUseCase, TDto>(TDto dto)
        where TUseCase : IUseCase<TDto>
        where TDto : EntityDto
    {
        return container.Resolve<TUseCase>(new TypedParameter(typeof(TDto), dto));
    }

    public IUseCase<TDto, TDtoReturn> ResolveUseCase<TUseCase, TDto, TDtoReturn>(TDto dto)
        where TUseCase : IUseCase<TDto, TDtoReturn>
        where TDto : EntityDto
        where TDtoReturn : EntityDto
    {
        return container.Resolve<TUseCase>(new TypedParameter(typeof(TDto), dto));
    }

    public IAuditableUseCase<TDto> ResolveAuditableUseCase<TUseCase, TDto>(TDto dto, User user)
        where TUseCase : IAuditableUseCase<TDto>
        where TDto : AuditableEntityDto
    {
        return container.Resolve<TUseCase>(new TypedParameter(typeof(TDto), dto), new TypedParameter(typeof(User), user));
    }

    public IAuditableUseCase<TDto, TDtoReturn> ResolveAuditableUseCase<TUseCase, TDto, TDtoReturn>(TDto dto, User user)
        where TUseCase : IAuditableUseCase<TDto, TDtoReturn>
        where TDto : AuditableEntityDto
        where TDtoReturn : EntityDto
    {
        return container.Resolve<TUseCase>(new TypedParameter(typeof(TDto), dto), new TypedParameter(typeof(User), user));
    }

    public IQueryUseCase<TLoadOptions> ResolveQueryUseCase<TUseCase, TLoadOptions>(TLoadOptions loadOptions)
        where TUseCase : IQueryUseCase<TLoadOptions>
        where TLoadOptions : DataSourceLoadOptionsBase
    {
        return container.Resolve<TUseCase>(new TypedParameter(typeof(TLoadOptions), loadOptions));
    }

    public ILookupUseCase<TEntity> ResolveLookupUseCase<TEntity>(LookupDataSourceLoadOptions loadOptions)
        where TEntity : Entity

    {
        return container.Resolve<ILookupUseCase<TEntity>>(new TypedParameter(typeof(LookupDataSourceLoadOptions), loadOptions));
    }

    public ILookupQueryUseCase<TLookupDto, TEntity> ResolveLookupQueryUseCase<TLookupDto, TEntity>(DataSourceLoadOptionsBase loadOptions)
        where TLookupDto : EntityDto<long>
        where TEntity : Entity

    {
        return container.Resolve<ILookupQueryUseCase<TLookupDto, TEntity>>(new TypedParameter(typeof(DataSourceLoadOptionsBase), loadOptions));
    }
}