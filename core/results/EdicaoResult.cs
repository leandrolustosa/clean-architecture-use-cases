public class EdicaoResult<TEntity> : SingleResult<TEntity>
    where TEntity : Entity
{
    public EdicaoResult()
    {
        Sucesso = true;
        Mensagem = MensagensNegocio.MSG02;
    }

    public EdicaoResult(TEntity entity)
        : this()
    {
        this.Data = entity;
    }

    public EdicaoResult(bool sucesso, string mensagem)
    {
        this.Sucesso = sucesso;
        this.Mensagem = mensagem;
    }

    public EdicaoResult(string mensagem)
    {
        this.Sucesso = false;
        this.Mensagem = mensagem;
    }
}