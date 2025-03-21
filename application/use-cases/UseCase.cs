public abstract class UseCase<TDto> : IUseCase<TDto>
    where TDto : EntityDto
{
    protected readonly IUnitOfWork uow;

    protected readonly IMapper mapper;

    protected readonly TDto dto;

    protected UseCase(TDto dto, IMapper mapper, IUnitOfWork uow)
    {
        this.dto = dto;
        this.uow = uow;
        this.mapper = mapper;
    }

    public abstract Task<ISingleResultDto<TDto>> ExecuteAsync();

    public Task<bool> CommitAsync()
    {
        return uow.CommitAsync();
    }
}
