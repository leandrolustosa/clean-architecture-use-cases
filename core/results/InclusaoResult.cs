public class InclusaoResult<TEntity> : SingleResult<TEntity>
    where TEntity : Entity
{
    public InclusaoResult(TEntity data)
    {
        Sucesso = true;
        Mensagem = MensagensNegocio.ResourceManager.GetString("MSG01");
        Data = data;
    }

    public InclusaoResult(bool sucesso, string mensagem)
    {
        this.Sucesso = sucesso;
        this.Mensagem = mensagem;
    }
    public InclusaoResult(string mensagem, TEntity data)
    {
        this.Sucesso = true;
        this.Mensagem = mensagem;
        this.Data = data;
    }
    public InclusaoResult(string mensagem)
    {
        this.Sucesso = false;
        this.Mensagem = mensagem;
    }
}