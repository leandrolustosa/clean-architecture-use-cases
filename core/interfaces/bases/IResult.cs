public interface IResult
{
	EnumResultadoAcao CodigoInterno { get; set; }
	IFluxoAlternativoResult FluxoAlternativo { get; set; }
	bool Sucesso { get; set; }
	string Mensagem { get; set; }
}