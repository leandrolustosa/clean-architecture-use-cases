[Route("api/[controller]/[action]")]
public class GenericBaseController<TEntity, TLoadOptions> : AppBaseController
    where TEntity : AuditableEntity
    where TLoadOptions : DataSourceLoadOptions
{
    public GenericBaseController(IUseCaseFacade facade) : base(facade) { }

    [HttpGet]
    public async Task<IActionResult> Lookup(LookupDataSourceLoadOptions options)
    {
        var result = await Facade.LookupAsync<TEntity>(options);
        return result.Sucesso ? Json(result.Data) : CreateErrorResponse(result);
    }

    [NonAction]
    public async Task<IActionResult> LookupQuery<TLookupDto>(LookupDataSourceLoadOptions options)
        where TLookupDto : EntityDto<long>
    {
        var result = await Facade.LookupQueryAsync<TLookupDto, TEntity, LookupDataSourceLoadOptions>(options);
        return result.Sucesso ? Json(result.Data) : CreateErrorResponse(result);
    }

    [NonAction]
    protected virtual async Task<IActionResult> List<TDto>(TLoadOptions options)
        where TDto : EntityDto
    {
        return await List<IListarUseCase<TEntity, TDto, TLoadOptions>, TDto>(options);
    }

    [NonAction]
    protected virtual async Task<IActionResult> List<TUseCase, TDto>(TLoadOptions options)
        where TUseCase : IQueryUseCase<TLoadOptions>
        where TDto : EntityDto
    {
        var result = await Facade.QueryAsync<TUseCase, TLoadOptions>(options);

        if (result.Sucesso)
        {
            return Ok(result.Data);
        }

        return CreateErrorResponse(result);
    }

    [NonAction]
    protected virtual async Task<ILoadResultDto> GetData<TDto>(TLoadOptions options)
        where TDto : EntityDto
    {
        var result = await Facade.QueryAsync<IListarUseCase<TEntity, TDto, TLoadOptions>, TLoadOptions>(options);
        
        return result;
    }
    [NonAction]
    protected virtual async Task<IActionResult> Get<TDto>(TDto dto)
        where TDto : EntityDto
    {
        var result = await Facade.ExecuteAsync<IGetSingleUseCase<TEntity, TDto>, TDto>(dto);

        if (result.Sucesso)
        {
            return Ok(result.Data);
        }

        return CreateErrorResponse(result);
    }

    [NonAction]
    protected virtual async Task<IActionResult> Insert<TDto>(TDto dto)
        where TDto : AuditableEntityDto
    {
        return await Insert<IIncluirUseCase<TEntity, TDto>, TDto>(dto);
    }

    [NonAction]
    protected virtual async Task<IActionResult> Insert<TUseCase, TDto>(TDto dto)
        where TUseCase : IAuditableUseCase<TDto>
        where TDto : AuditableEntityDto
    {
        if (!ModelState.IsValid)
        {
            var errorResult = new SingleResultDto<TDto>(MensagensAplicacao.MENSAGEM_ERRO_VALIDACAO);
            return CreateModelStateError(errorResult);
        }

        var result = await Facade.ExecuteAuditableAsync<TUseCase, TDto>(dto, LoggedUser);

        if (result.Sucesso || result.FluxoAlternativo != null)
        {
            if (!dto.InibirMensagens)
            {
                SetMessage(result.Mensagem, "success");
            }
            return Ok(result);
        }

        if (!dto.InibirMensagens)
        {
            SetMessage(result.Mensagem, "error");
        }
        return CreateErrorResponse(result);
    }

    [NonAction]
    protected virtual async Task<IActionResult> Update<TDto>(TDto dto)
        where TDto : AuditableEntityDto
    {
        return await Update<IEditarUseCase<TEntity, TDto>, TDto>(dto);
    }

    [NonAction]
    protected virtual async Task<IActionResult> Update<TUseCase, TDto>(TDto dto)
        where TUseCase : IAuditableUseCase<TDto>
        where TDto : AuditableEntityDto
    {
        if (!ModelState.IsValid)
        {
            var errorResult = new SingleResultDto<TDto>(MensagensAplicacao.MENSAGEM_ERRO_VALIDACAO);
            return CreateModelStateError(errorResult);
        }

        var result = await Facade.ExecuteAuditableAsync<TUseCase, TDto>(dto, LoggedUser);

        if (result.Sucesso || result.FluxoAlternativo != null)
        {
            if (!dto.InibirMensagens)
            {
                SetMessage(result.Mensagem, "success");
            }
            return Ok(result);
        }

        if (!dto.InibirMensagens)
        {
            SetMessage(result.Mensagem, "error");
        }

        return CreateErrorResponse(result);
    }

    [NonAction]
    protected virtual async Task<IActionResult> Remove<TDto>(TDto dto)
        where TDto : AuditableEntityDto
    {
        return await Remove<IExcluirUseCase<TEntity, TDto>, TDto>(dto);
    }

    [NonAction]
    protected virtual async Task<IActionResult> Remove<TUseCase, TDto>(TDto dto)
        where TUseCase : IAuditableUseCase<TDto>
        where TDto : AuditableEntityDto
    {
        if (!ModelState.IsValid)
        {
            var errorResult = new SingleResultDto<TDto>(MensagensAplicacao.MENSAGEM_ERRO_VALIDACAO);
            return CreateModelStateError(errorResult);
        }

        var result = await Facade.ExecuteAuditableAsync<TUseCase, TDto>(dto, LoggedUser);

        if (result.Sucesso || result.FluxoAlternativo != null)
        {
            if (!dto.InibirMensagens)
            {
                SetMessage(result.Mensagem, "success");
            }
            return Ok(result);
        }

        if (!dto.InibirMensagens)
        {
            SetMessage(result.Mensagem, "error");
        }
        return CreateErrorResponse(result);
    }
}