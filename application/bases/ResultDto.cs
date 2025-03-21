public class ResultDto : IResultDto
{
    [JsonIgnore]
    public EnumResultadoAcao CodigoInterno { get; set; }
    public int Codigo { get { return (int)CodigoInterno; } }
    public bool Sucesso { get; set; }
    public string Mensagem { get; set; }
    public IList<string> Mensagens { get; set; }
    public IFluxoAlternativoResult FluxoAlternativo { get; set; }

    public ValidationResult ValidationResult { get; set; }

    protected string GetExceptionMessages(Exception e, string msgs = "", int nivel = 0)
    {
        if (e == null) return !string.IsNullOrEmpty(msgs) ? msgs : "Erro inesperado, por favor entre em contato o suporte";

        if (e is AppException && nivel == 0)
        {
            msgs = e.Message;
        }
        else
        {
            if (msgs == "" || nivel == 0)
            {
                msgs += e.Message;
            }

            if (e.InnerException != null)
            {
                msgs += "\r\nInnerException: " + GetExceptionMessages(e.InnerException, nivel: nivel + 1);
            }
        }

        return msgs;
    }
}