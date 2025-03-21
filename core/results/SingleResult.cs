public class SingleResult<TEntity> : ISingleResult<TEntity>
    where TEntity : Entity
{
    public SingleResult()
    {
        this.CodigoInterno = EnumResultadoAcao.Sucesso;
        this.Sucesso = true;
    }

    public SingleResult(string mensagem)
    {
        this.CodigoInterno = EnumResultadoAcao.ErroValidacaoNegocio;
        this.Sucesso = false;
        this.Mensagem = mensagem;
    }

    public SingleResult(bool sucesso, string mensagem)
    {
        this.CodigoInterno = sucesso ? EnumResultadoAcao.Sucesso : EnumResultadoAcao.ErroValidacaoNegocio;
        this.Sucesso = sucesso;
        this.Mensagem = mensagem;
    }

    public SingleResult(Exception ex)
    {
        this.CodigoInterno = EnumResultadoAcao.ErroServidor;
        this.Sucesso = false;
        this.Mensagem = GetExceptionMessages(ex, MensagensNegocio.MSG07);
    }

    public SingleResult(TEntity data)
    {
        this.CodigoInterno = (data == null) ? EnumResultadoAcao.ErroNaoEncontrado : EnumResultadoAcao.Sucesso;
        this.Sucesso = data != null;
        this.Mensagem = (data == null) ? MensagensNegocio.ResourceManager.GetString("MSG04") : string.Empty;
        this.Data = data;
    }

    public SingleResult(TEntity data, string mensagem)
        : this(data)
    {
        this.Mensagem = (data == null) ? MensagensNegocio.ResourceManager.GetString("MSG04") : mensagem;
    }

    public SingleResult(IFluxoAlternativoResult fluxoAlternativo)
    {
        this.CodigoInterno = EnumResultadoAcao.FluxoAlternativo;
        this.Sucesso = false;
        this.FluxoAlternativo = fluxoAlternativo;
    }

    public IFluxoAlternativoResult FluxoAlternativo { get; set; }
    public EnumResultadoAcao CodigoInterno { get; set; }
    public bool Sucesso { get; set; }
    public string Mensagem { get; set; }
    public TEntity Data { get; set; }


    protected string GetExceptionMessages(Exception e, string msgs = "", int nivel = 0)
    {
        if (e == null) return string.Empty;

        if (msgs == "" || nivel == 0)
        {
            msgs += e.Message;
        }

        if (e.InnerException != null)
        {
            msgs += "\r\nInnerException: " + GetExceptionMessages(e.InnerException, nivel: nivel + 1);
        }

        return msgs;
    }
}