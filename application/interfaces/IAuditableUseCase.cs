public interface IAuditableUseCase<TDto>
    where TDto : AuditableEntityDto
{
    IAuditContext AuditContext { get; set; }

    Task<ISingleResultDto<TDto>> ExecuteAsync();
}
