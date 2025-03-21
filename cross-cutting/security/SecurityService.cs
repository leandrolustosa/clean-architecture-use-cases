public class SecurityService : ISecurityService
{
    private readonly ISecurityEndPoint41 client;    
    private readonly ControleAcessoConfig config;
    private readonly ILogger<SecurityService> logger;

    public SecurityService(ISecurityEndPoint41 client, ControleAcessoConfig config, ILogger<SecurityService> logger)
    {
        this.client = client;        
        this.config = config;
        this.logger = logger;
    }

    public async Task<ServiceResult<User>> LoginAsync(string chave, string senha)
    {
        soapMessageHeader messageHeader = null;

        try
        {
            var request = new logonRequest
            {
                messageHeader = messageHeader,
                regionalId = config.RegionalId,
                environmentId = config.EnvironmentId,
                applicationCatalogId = config.ApplicationCatalogId,
                applicationPassword = config.ApplicationPassword,
                userLogin = chave,
                userPassword = senha,
                isInternalUser = true
            };
            
            var output = await client.logonAsync(request);

            var result = await GetUserResultAsync(output.@return);

            if (!result.Success)
            {
                return result;
            }

            var value = (TokenUserDTO)output.@return.value;

            var messageHeaderPesquisa = new soapMessageHeader
            {
                executionLanguageCode = value.supportedLanguages[0].locale.languageCode,
                ticket = value.ticket
            };

            var perfis = await GetRolesOfUserAsync(client, messageHeaderPesquisa, chave);
            
            var recursos = await GetResourcesByRolesAsync(client, messageHeaderPesquisa, perfis);
            
            result.Data.Roles = perfis;
            result.Data.Resources = recursos;

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError("Error " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ex.ToString()));
            var errorCode = "500";
            var errorMessage = (ex.InnerException != null) ? ex.InnerException.ToString() : ex.ToString();
            var result = new ServiceResult<User>(errorCode, errorMessage);

            return result;
        }
    }

    public async Task<ServiceResult<User>> GetByKeyAsync(string chave, string sessionId)
    {   
        try
        {
            var request = new findUserByLoginRequest
            {
                messageHeader = new soapMessageHeader
                {
                    ticket = new TicketDTO { sessionId = sessionId }
                },
                userLogin = chave
            };

            var output = await client.findUserByLoginAsync(request);

            var message = output.@return;

            if (message.exceptionInfo != null)
            {
                var errorCode = message.exceptionInfo.code;
                var errorMessage = message.exceptionInfo.message;

                return new ServiceResult<User>(errorCode.ToString(), errorMessage);
            }

            if (message.value == null)
            {
                return new ServiceResult<User>("404", "Chave de usuário não encontrada.");
            }

            var worker = (ContractedWorkerUserDTO)message.value;

            var user = new User
            {
                Chave = worker.login,
                Nome = worker.name
            };

            return new ServiceResult<User>(user);
        }
        catch (Exception ex)
        {
            logger.LogError("Error " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ex.ToString()));

            var errorMessage = (ex.InnerException != null) ? ex.InnerException.ToString() : ex.ToString();
            return new ServiceResult<User>("500", errorMessage);
        }
    }

    private async Task<ServiceResult<User>> GetUserResultAsync(soapReturnMessage message)
    {
        if (message.exceptionInfo != null || message.value == null)
        {
            var errorCode = message.exceptionInfo.code;
            var errorMessage = message.exceptionInfo.message;
            var result = new ServiceResult<User>(errorCode.ToString(CultureInfo.CurrentCulture), errorMessage);

            return result;
        }

        try
        {
            var token = GetToken(message);

            var user = new User
            {
                Chave = token.user.login,
                Nome = workforceResult.Data.Nome,
                SessionId = token.ticket.sessionId
            };


            var result = new ServiceResult<User>(user);

            return result;

        }
        catch (Exception ex)
        {
            logger.LogError("Error " + ((ex.InnerException != null) ? ex.InnerException.ToString() : ex.ToString()));
            var errorCode = "500";
            var errorMessage = (ex.InnerException != null) ? ex.InnerException.ToString() : ex.ToString();
            var result = new ServiceResult<User>(errorCode, errorMessage);

            return result;
        }
    }

    private TokenUserDTO GetToken(soapReturnMessage message)
    {
        if (message != null && message.value != null)
        {
            return (TokenUserDTO)message.value;
        }

        return null;
    }

    public async Task<ServiceResult<IList<string>>> GetUsersByRoleAsync(soapMessageHeader messageHeader, string role)
    {
        try
        {
            var client = new SecurityEndPoint41Client();
            var output = await client.findAllRolesOfUserAsync(messageHeader, role);

            if (output.@return.exceptionInfo == null && output.@return.value != null)
            {
                var roles = output.@return.value.OfType<RoleDTO>().Select(p => p.id).ToList();
                if (roles != null && roles.Count == 0)
                {
                    return new ServiceResult<IList<string>>(new List<string> { "Usuario" });
                }

                return new ServiceResult<IList<string>>(roles);
            }
        }
        catch (Exception)
        {
            return new ServiceResult<IList<string>>(new List<string> { });
        }

        return new ServiceResult<IList<string>>(new List<string> { "Usuario" });
    }

    public async Task<ServiceResult<Dictionary<string, string>>> GetUsersByRoleAsync(string sessionId, string[] roles)
    {
        var data = new Dictionary<string, string>();

        foreach (string role in roles.Distinct())
        {
            try
            {
                var request = new findAllUserAuthorizationsOfRoleRequest
                {
                    messageHeader = new soapMessageHeader
                    {
                        ticket = new TicketDTO { sessionId = sessionId }
                    },
                    roleId = role
                };
                var output = await client.findAllUserAuthorizationsOfRoleAsync(request);

                if (output.@return.exceptionInfo == null && output.@return.value != null)
                {
                    var users = output.@return.value.OfType<UserRoleAuthorizationDTO>().Select(p => p.user).ToList();
                    if (users != null && users.Count > 0)
                    {
                        data.Add(role, string.Join(",", users.Select(u => u.login).ToArray()));
                    }
                }
            }
            catch (Exception)
            {
                data.Add(role, "");
            }

            if (!data.ContainsKey(role))
            {
                data.Add(role, "");
            }
        }

        return new ServiceResult<Dictionary<string, string>>(data);
    }

    private async Task<IList<string>> GetRolesOfUserAsync(ISecurityEndPoint41 client, soapMessageHeader messageHeader, string chave)
    {
        try
        {
            var request = new findAllRolesOfUserRequest
            {
                messageHeader = messageHeader,
                userLogin = chave
            };

            var output = await client.findAllRolesOfUserAsync(request);

            if (output.@return.exceptionInfo == null && output.@return.value != null)
            {
                var roles = output.@return.value.OfType<RoleDTO>().Select(p => p.id).ToList();
                if (roles != null && roles.Count == 0)
                {
                    return new List<string> { "Usuario" };
                }

                return roles;
            }
        }
        catch (Exception)
        {
            return new List<string>();
        }

        return new List<string> { "Usuario" };
    }

    private async Task<IList<string>> GetResourcesByRolesAsync(ISecurityEndPoint41 client, soapMessageHeader messageHeader, IList<string> roles)
    {
        var resources = new List<string>();

        try
        {
            foreach (var roleId in roles.Where(r => r != "Usuario").ToList())
            {
                var request = new findAllResouceAuthorizationsOfRoleRequest
                {
                    messageHeader = messageHeader,
                    roleId = roleId
                };

                var output = await client.findAllResouceAuthorizationsOfRoleAsync(request);

                if (output.@return.exceptionInfo == null && output.@return.value != null)
                {
                    var recursos = output.@return.value.OfType<RoleResourceAuthorizationDTO>().Select(p => p.resource.id).ToList();

                    resources.AddRange(recursos);
                }
            }

            resources = resources.Distinct().ToList();
        }
        catch (Exception)
        {
            return resources;
        }

        return resources;
    }
}