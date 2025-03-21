public interface ISecurityService
{
    Task<ServiceResult<User>> LoginAsync(string chave, string senha);

    Task<ServiceResult<User>> GetByKeyAsync(string chave, string sessionId);

    Task<ServiceResult<Dictionary<string, string>>> GetUsersByRoleAsync(string sessionId, string[] roles);

}