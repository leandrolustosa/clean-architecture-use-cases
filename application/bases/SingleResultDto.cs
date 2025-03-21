public class SingleResultDto<TDto> : ResultDto, ISingleResultDto<TDto>
    where TDto : Dto
{
    public SingleResultDto()
    {
        CodigoInterno = EnumResultadoAcao.Sucesso;
        Sucesso = true;
        Mensagens = new List<string>();
    }

    public SingleResultDto(TDto data)
    {
        CodigoInterno = data == null ? EnumResultadoAcao.ErroNaoEncontrado : EnumResultadoAcao.Sucesso;
        Sucesso = data != null;
        Mensagem = data == null ? MensagensNegocio.ResourceManager.GetString("MSG04") : string.Empty;
        Data = data;
    }

    public SingleResultDto(string mensagem)
    {
        CodigoInterno = EnumResultadoAcao.ErroValidacaoNegocio;
        Sucesso = false;
        Mensagem = mensagem;
    }

    public SingleResultDto(bool sucesso, string mensagem)
    {
        CodigoInterno = sucesso ? EnumResultadoAcao.Sucesso : EnumResultadoAcao.ErroValidacaoNegocio;
        Sucesso = sucesso;
        Mensagem = mensagem;
    }

    public SingleResultDto(EnumResultadoAcao tipoErro, string mensagem)
        : this(mensagem)
    {
        CodigoInterno = tipoErro;
    }

    public SingleResultDto(Exception ex)
    {
        CodigoInterno = EnumResultadoAcao.ErroServidor;
        Sucesso = false;
        Mensagem = GetExceptionMessages(ex, MensagensNegocio.MSG07);
    }

    public SingleResultDto(IResult result)
    {
        CodigoInterno = result.CodigoInterno;
        Sucesso = result.Sucesso;
        Mensagem = result.Mensagem;
        FluxoAlternativo = result.FluxoAlternativo;
    }

    public TDto Data { get; private set; }

    public void SetData<TEntity>(ISingleResult<TEntity> result, IMapper mapper)
        where TEntity : Entity
    {
        Data = mapper.Map<TDto>(result.Data);
    }
}