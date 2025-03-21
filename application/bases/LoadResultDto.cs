public class LoadResultDto : ResultDto, ILoadResultDto		
{
    public LoadResultDto(LoadResult data)
    {
        Sucesso = data != null && data.data != null;
        Data = data;
    }

    public LoadResultDto(Exception ex)
    {
        CodigoInterno = EnumResultadoAcao.ErroServidor;
        Sucesso = false;
        Mensagem = MensagensNegocio.MSG07;
    }

    public LoadResult Data { get; set; }
}