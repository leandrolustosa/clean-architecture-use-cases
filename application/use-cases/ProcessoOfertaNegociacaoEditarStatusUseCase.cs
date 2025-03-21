public class ProcessoOfertaNegociacaoEditarStatusUseCase<TDto> : AuditableUseCase<TDto>, IProcessoOfertaNegociacaoEditarStatusUseCase<TDto>
    where TDto : ProcessoOfertaNegociacaoEditarStatusDto
{
    private readonly IProcessoOfertaRepository _repository;
    private readonly IProcessoOfertaNegociacaoEditarStatusValidation<TDto> _validation;
    public ProcessoOfertaNegociacaoEditarStatusUseCase(TDto dto, User user, IAuditContext auditContext, IMapper mapper, IUnitOfWork uow, IProcessoOfertaRepository repository, IProcessoOfertaNegociacaoEditarStatusValidation<TDto> validation) : base(dto, user, auditContext, mapper, uow)
    {
        _repository = repository;
        _validation = validation;
    }

    public override async Task<ISingleResultDto<TDto>> ExecuteAsync()
    {
        var registroExisteValidation = await _validation.RegistroExisteAsync(dto.Id, nameof(ProcessoOferta.ProcessoOfertaNegociacao));
        if (!registroExisteValidation.Sucesso)
        {
            return new SingleResultDto<TDto>(registroExisteValidation);
        }

        var resultValidation = await _validation.ValidarAsync(registroExisteValidation.Data, dto);
        if (!resultValidation.Sucesso)
        {
            return new SingleResultDto<TDto>(resultValidation);
        }

        var processoOferta = mapper.Map(dto, registroExisteValidation.Data);

        var acao = ObterTipoAcao(processoOferta.IdTipoSituacaoOferta);
        if (!acao.Sucesso)
        {
            return new SingleResultDto<TDto>(acao.Mensagem);
        }

        if (dto is ProcessoOfertaNegociacaoEditarStatusOnSubsDto)
        {
            AtualizarOfertaOnSubs(processoOferta);
        }

        _repository.Update(processoOferta);
        await CommitAsync();

        var result = new EdicaoResult<SolicitacaoAfretamento>(true, acao.Data.Mensagem);
        var resultDto = new SingleResultDto<TDto>(result);

        resultDto.SetData(result, mapper);
        return resultDto;
    }

    private void AtualizarOfertaOnSubs(ProcessoOferta processoOferta)
    {
        var ultimaNegociacao = processoOferta.ProcessoOfertaNegociacao.OrderBy(o => o.NumeroRodada).ThenByDescending(o => o.OrigemOferta).LastOrDefault();
        if (ultimaNegociacao != null)
        {
            foreach (var item in processoOferta.ProcessoOfertaNegociacao)
            {
                item.IndicadorAtivo = false;
            }

            ultimaNegociacao.IndicadorAtivo = true;
        }
    }

    private ISingleResultDto<TipoAcaoDto> ObterTipoAcao(long idTipoSituacaoOferta)
    {
        switch ((EnumSituacaoProcessoOferta)idTipoSituacaoOferta)
        {
            case EnumSituacaoProcessoOferta.EmNegociacao:
                return new SingleResultDto<TipoAcaoDto>(new TipoAcaoDto(TipoAcao.Alteracao, MensagensNegocio.MSG23));

            case EnumSituacaoProcessoOferta.NegociacaoEncerradaPelaContraparte:
                return new SingleResultDto<TipoAcaoDto>(new TipoAcaoDto(TipoAcao.Alteracao, MensagensNegocio.MSG24));

            case EnumSituacaoProcessoOferta.OnSubs:
                return new SingleResultDto<TipoAcaoDto>(new TipoAcaoDto(TipoAcao.Alteracao, MensagensNegocio.MSG25));

            default:
                return new SingleResultDto<TipoAcaoDto>(MensagensAplicacao.SITUACAO_NAO_PERMITIDA);
        }
    }
}