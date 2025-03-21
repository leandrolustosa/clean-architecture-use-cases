public abstract class UseCase<TDto, TDtoReturn> : IUseCase<TDto, TDtoReturn>
    where TDto : EntityDto
    where TDtoReturn : EntityDto
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

    public abstract Task<ISingleResultDto<TDtoReturn>> ExecuteAsync();

    public Task<bool> CommitAsync()
    {
        return uow.CommitAsync();
    }
}