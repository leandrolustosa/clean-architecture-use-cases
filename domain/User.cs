public class User
{
    public string Chave { get; set; }
    public string Nome { get; set; }    
    public string SessionId { get; set; }

    public IList<string> Roles { get; set; }
    public IList<string> Resources { get; set; }
}