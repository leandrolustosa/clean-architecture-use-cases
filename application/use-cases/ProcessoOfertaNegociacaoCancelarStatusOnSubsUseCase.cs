public class ProcessoOfertaNegociacaoCancelarStatusOnSubsUseCase : AuditableUseCase<ProcessoOfertaNegociacaoCancelarStatusOnSubsDto>, IProcessoOfertaNegociacaoCancelarStatusOnSubsUseCase
{
    private readonly IProcessoOfertaRepository _repository;
    private readonly IRepository<ProcessoOfertaDetalhe> _ofertaDetalheRepository;
    private readonly IProcessoAberturaRepository _aberturaRepository;
    private readonly IRepository<ProcessoFechamento> _fechamentoRepository;
    private readonly IRepository<ProcessoFechamentoHistorico> _fechamentoHistoricoRepository;
    private readonly IProcessoDeclaracaoRepository _declaracaoRepository;
    private readonly IProcessoOfertaNegociacaoCancelarStatusOnSubsValidation _validation;

    public ProcessoOfertaNegociacaoCancelarStatusOnSubsUseCase(ProcessoOfertaNegociacaoCancelarStatusOnSubsDto dto,
        User user, IAuditContext auditContext, IMapper mapper, IUnitOfWork uow, IProcessoOfertaRepository repository,
        IProcessoAberturaRepository aberturaRepository, IRepository<ProcessoFechamento> fechamentoRepository,
        IRepository<ProcessoFechamentoHistorico> fechamentoHistoricoRepository, IProcessoDeclaracaoRepository declaracaoRepository,
        IProcessoOfertaNegociacaoCancelarStatusOnSubsValidation validation, IRepository<ProcessoOfertaDetalhe> ofertaDetalheRepository)
        : base(dto, user, auditContext, mapper, uow)
    {
        _repository = repository;
        _aberturaRepository = aberturaRepository;
        _fechamentoRepository = fechamentoRepository;
        _fechamentoHistoricoRepository = fechamentoHistoricoRepository;
        _declaracaoRepository = declaracaoRepository;
        _validation = validation;
        _ofertaDetalheRepository = ofertaDetalheRepository;
    }

    public override async Task<ISingleResultDto<ProcessoOfertaNegociacaoCancelarStatusOnSubsDto>> ExecuteAsync()
    {
        var registroExisteValidation = await _validation.RegistroExisteAsync(dto.Id,
            nameof(ProcessoOferta.ProcessoAbertura),
            nameof(ProcessoOferta.ProcessoOfertaNegociacao));

        if (!registroExisteValidation.Sucesso)
        {
            return new SingleResultDto<ProcessoOfertaNegociacaoCancelarStatusOnSubsDto>(registroExisteValidation);
        }

        var resultValidation = await _validation.ValidarAsync(registroExisteValidation.Data, dto);
        if (!resultValidation.Sucesso)
        {
            return new SingleResultDto<ProcessoOfertaNegociacaoCancelarStatusOnSubsDto>(resultValidation);
        }

        var processoOferta = mapper.Map(dto, registroExisteValidation.Data);

        await ExcluirDadosAsync(processoOferta.IdProcessoAbertura, processoOferta);

        AtualizarOfertaOnSubs(processoOferta);

        ConfigureAuditContext(processoOferta, processoOferta.ProcessoAbertura.ProcessoAfretamento.CodigoProcesso);
        _repository.Update(processoOferta);

        await CommitAsync();

        var result = new EdicaoResult<SolicitacaoAfretamento>(true, MensagensNegocio.MSG49);
        var resultDto = new SingleResultDto<ProcessoOfertaNegociacaoCancelarStatusOnSubsDto>(result);

        resultDto.SetData(result, mapper);
        return resultDto;
    }

    private void AtualizarOfertaOnSubs(ProcessoOferta processoOferta)
    {
        var ultimaNegociacao = processoOferta.ProcessoOfertaNegociacao.OrderBy(o => o.NumeroRodada).ThenByDescending(o => o.OrigemOferta).Where(x => !x.OrigemOferta.Equals("P")).LastOrDefault();
        if (ultimaNegociacao != null)
        {
            foreach (var item in processoOferta.ProcessoOfertaNegociacao)
            {
                ConfigureAuditContext(item, processoOferta.ProcessoAbertura.ProcessoAfretamento.CodigoProcesso);
                item.IndicadorAtivo = false;
            }

            ultimaNegociacao.IndicadorAtivo = true;
        }
    }

    private async Task ExcluirDadosAsync(long idProcessoAbertura, ProcessoOferta po)
    {
        var abertura = await _aberturaRepository.GetByIdAsync(idProcessoAbertura,
            nameof(ProcessoAbertura.ProcessoAfretamento),
            nameof(ProcessoAbertura.ProcessoFechamento),
            nameof(ProcessoAbertura.ProcessosDeclaracao));

        if (abertura.ProcessoFechamento != null && abertura.ProcessoFechamento.Any())
        {
            foreach (var fechamento in abertura.ProcessoFechamento)
            {
                await ExcluirDadosHistoricoFechamento(fechamento, abertura);

                ConfigureAuditContext(fechamento, abertura.ProcessoAfretamento.CodigoProcesso);
                _fechamentoRepository.Remove(fechamento);
            }
        }

        if (abertura.ProcessosDeclaracao != null && abertura.ProcessosDeclaracao.Any())
        {
            foreach (var declaracao in abertura.ProcessosDeclaracao)
            {
                ConfigureAuditContext(declaracao, abertura.ProcessoAfretamento.CodigoProcesso);
                _declaracaoRepository.Remove(declaracao);
            }
        }

        if (po.ProcessoOfertaDetalhe != null && po.ProcessoOfertaDetalhe.Any())
        {
            foreach (var ofertaDetalhe in po.ProcessoOfertaDetalhe)
            {
                ofertaDetalhe.IdPortoAfretamento = null;
                ofertaDetalhe.IdPortoTaxa = null;
                ofertaDetalhe.IndicadorNegociado = false;
                ofertaDetalhe.NomeUnidadeMedida = null;
                ofertaDetalhe.QuantidadeTempoDescarga = null;
                ofertaDetalhe.ValorFrete = null;
                ofertaDetalhe.ValorTaxaFixa = null;
                ofertaDetalhe.ValorTotalDolar = null;
                ofertaDetalhe.IndicadorEstadiaAlterada = false;

                ConfigureAuditContext(ofertaDetalhe, abertura.ProcessoAfretamento.CodigoProcesso);
                _ofertaDetalheRepository.Update(ofertaDetalhe);
            }
        }

        abertura.IndicadorAprovacaoGerencial = "NI";
        abertura.IndicadorAprovacaoVetting = "NI";
        abertura.IndicadorAprovacaoCarga = "NI";
        abertura.IndicadorAprovacaoRecebedores = "NI";
        abertura.IndicadorAprovacaoEmbarcadores = "NI";
        abertura.IndicadorDeclaracaoSacGerada = false;
        abertura.DataContrato = null;
        abertura.DataAprovacaoGerencial = null;

        ConfigureAuditContext(abertura, abertura.ProcessoAfretamento.CodigoProcesso);
        _aberturaRepository.Update(abertura);
    }

    private async Task ExcluirDadosHistoricoFechamento(ProcessoFechamento fechamento, ProcessoAbertura abertura)
    {
        var historicos = await _fechamentoHistoricoRepository.Buscar(p => p.IdProcessoFechamento == fechamento.Id).ToListAsync();

        foreach (var historico in historicos)
        {
            ConfigureAuditContext(historico, abertura.ProcessoAfretamento.CodigoProcesso);
            _fechamentoHistoricoRepository.Remove(historico);
        }
    }
}