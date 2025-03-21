public class ProcessoOfertaNegociacaoController : GenericBaseController<ProcessoOfertaNegociacao, DataSourceLoadOptions>
{
    public ProcessoOfertaNegociacaoController(IUseCaseFacade facade) : base(facade)
    {
    }

    [HttpGet]
    public async Task<IActionResult> Listar(DataSourceLoadOptions options)
    {
        return await List<ProcessoOfertaNegociacaoDto>(options);
    }

    [HttpPut]
    public async Task<IActionResult> Editar([FromBody] ProcessoOfertaNegociacaoEditarDto dto)
    {
        return await Update<IProcessoOfertaNegociacaoEditarUseCase, ProcessoOfertaNegociacaoEditarDto>(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Incluir([FromBody] ProcessoOfertaNegociacaoIncluirDto dto)
    {
        return await Insert<IProcessoOfertaNegociacaoIncluirUseCase, ProcessoOfertaNegociacaoIncluirDto>(dto);
    }

    [HttpDelete]
    public async Task<IActionResult> Excluir([FromQuery] ProcessoOfertaNegociacaoExcluirDto dto)
    {
        return await Remove<IProcessoOfertaNegociacaoExcluirUseCase, ProcessoOfertaNegociacaoExcluirDto>(dto);
    }

    [HttpGet]
    public async Task<IActionResult> Obter([FromQuery] ProcessoOfertaNegociacaoListarDto dto)
    {
        return await Get(dto);
    }

    [HttpPost]
    public async Task<IActionResult> CalcularValorOferta([FromBody] CalculoValorOfertaDto dto)
    {
        var result = await Facade.ExecuteAsync<ICalcularValorOfertaUseCase, CalculoValorOfertaDto>(dto);

        if (result.Sucesso)
        {
            return Ok(result);
        }

        return CreateErrorResponse(result);
    }
    [HttpPost]
    public async Task<IActionResult> IniciarNegociacao([FromBody] ProcessoOfertaIniciarNegociacaoDto dto)
    {
        var result = await Facade.ExecuteAuditableAsync<IProcessoOfertaIniciarNegociacaoUseCase, ProcessoOfertaIniciarNegociacaoDto, ListaOfertasRankingDto>(dto, LoggedUser);

        if (result.Sucesso)
        {
            return Ok(result);
        }

        return CreateErrorResponse(result);
    }
    [HttpPost]
    public async Task<IActionResult> PermiteInicioNegociacao([FromBody] ListaOfertasRankingDto dto)
    {
        var result = await Facade.ExecuteAsync<IProcessoOfertaValidaInicioNegociacaoUseCase, ListaOfertasRankingDto>(dto);

        return Ok(result);

    }
}