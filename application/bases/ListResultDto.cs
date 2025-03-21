public class ListResultDto<T> : ResultDto, IListResultDto<T>
    where T: Dto
{
    public ListResultDto(IEnumerable<T> list)
    {
        Sucesso = true;
        Data = list;
    }

    public ListResultDto()
    {
        CodigoInterno = EnumResultadoAcao.ErroServidor;
        Sucesso = false;
        Mensagem = MensagensNegocio.MSG07;
    }

    public ListResultDto(Exception ex)
    {
        CodigoInterno = EnumResultadoAcao.ErroServidor;
        Sucesso = false;
        Mensagem = string.Format("{0}. Erro: {1}", MensagensNegocio.MSG07, ex.Message);
    }

    public IEnumerable<T> Data { get; set; }
}