public class ExclusaoResult<TEntity> : SingleResult<TEntity>
	where TEntity : Entity
{
	public ExclusaoResult()
	{
		Sucesso = true;
		Mensagem = MensagensNegocio.MSG03;
	}

	public ExclusaoResult(bool sucesso, string mensagem)
	{
		this.Sucesso = sucesso;
		this.Mensagem = mensagem;
	}

	public ExclusaoResult(string mensagem)
	{
		this.Sucesso = false;
		this.Mensagem = mensagem;
	}
}