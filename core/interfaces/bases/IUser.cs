public interface IUser
{
    string Chave { get; }
    string Nome { get; }    
    string SessionId { get; }
    bool IsAuthenticated();
    IEnumerable<Claim> GetClaimsIdentity();
    bool HasClaim(string claimType, string claimValue);
    bool TemPermissao(EnumRecursos recurso);
}