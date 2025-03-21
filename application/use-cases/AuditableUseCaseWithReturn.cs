public abstract class AuditableUseCase<TDto, TDtoReturn> : IAuditableUseCase<TDto, TDtoReturn>
    where TDto : AuditableEntityDto
    where TDtoReturn : EntityDto
{
    protected readonly IUnitOfWork uow;

    protected readonly IMapper mapper;

    protected readonly TDto dto;

    protected User User { get; set; }

    public IAuditContext AuditContext { get; set; }

    protected AuditableUseCase(TDto dto, User user, IAuditContext auditContext, IMapper mapper, IUnitOfWork uow)
    {
        this.dto = dto;
        this.AuditContext = auditContext;
        this.User = user;
        this.uow = uow;
        this.mapper = mapper;
    }

    public abstract Task<ISingleResultDto<TDtoReturn>> ExecuteAsync();

    public Task<bool> CommitAsync()
    {
        return uow.CommitAsync();
    }

    protected void ConfigureAuditContext(AuditableEntity entity)
    {
        ConfigureAuditContext(entity, null, false);
    }

    protected void ConfigureAuditContext(AuditableEntity entity, string identificador)
    {
        ConfigureAuditContext(entity, identificador, false);
    }

    protected void ConfigureAuditContext(AuditableEntity entity, string identificador, bool clone)
    {
        entity.AuditContext = (clone) ? this.AuditContext.Clone(identificador) : this.AuditContext;
        entity.User = this.User;
        entity.AuditContext.Identificador = identificador;
    }

    protected void ConfigureAuditContext(AuditableEntity entity, string identificador, bool clone, EnumAcao acao)
    {
        entity.AuditContext = (clone) ? this.AuditContext.Clone(identificador) : this.AuditContext;
        entity.User = this.User;
        entity.AuditContext.Identificador = identificador;
        entity.AuditContext.Acao = acao;
    }
    protected void SetAuditContext(AuditableEntity entity)
    {
        this.AuditContext = entity.AuditContext;
        this.User = entity.User;
    }
}