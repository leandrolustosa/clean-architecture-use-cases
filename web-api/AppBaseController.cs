[Route("api/[controller]/[action]")]
[Authorize]
public class AppBaseController : Controller
{
    protected IUseCaseFacade Facade { get; }

    public AppBaseController(IUseCaseFacade facade)
    {
        Facade = facade;
    }

    [NonAction]
    protected IActionResult CreateErrorResponse<TDto>(TDto result)
        where TDto : IResultDto
    {
        switch (result.CodigoInterno)
        {
            case EnumResultadoAcao.ErroValidacaoNegocio:
                ModelState.AddModelError("Id", result.Mensagem);
                return CreateModelStateError(result);
            case EnumResultadoAcao.ErroNaoEncontrado:
                return NotFound(result);
            default:
                return StatusCode(500, result);
        }
    }

    [NonAction]
    protected IActionResult CreateModelStateError<TDto>(TDto result)
        where TDto : IResultDto
    {
        result.Mensagens = GetMessagesModelStateErrors();
        return BadRequest(result);
    }

    [NonAction]
    protected IList<string> GetMessagesModelStateErrors()
    {
        var errors = ModelState.Values.SelectMany(v => v.Errors);

        return errors.Select(error => error.Exception == null ? error.ErrorMessage : error.Exception.Message).ToList();
    }

    protected void SetMessage(string message, string type)
    {
        TempData["message"] = message;
        TempData["type"] = type;
    }

    protected User LoggedUser
    {
        get
        {
            return new User
            {
                Chave = User.Identity.Name?.ToUpper(),
                Nome = User.FindFirstValue("FullName"),
                SessionId = User.FindFirstValue("SessionId")
            };
        }
    }

    protected bool HasRecurso(EnumRecursos recurso)
    {
        return HasRecurso(recurso.ToString());
    }
    
    protected bool HasRecurso(string recurso)
    {
        return User.Claims.Any(claim => claim.Type == "Recurso" && claim.Value == recurso);
    }
    [NonAction]
    public RecursosUsuarioDto GetRecursosUsuario()
    {
        RecursosUsuarioDto recursosUsuario = new RecursosUsuarioDto();
        recursosUsuario.ChaveUsuarioLogado = LoggedUser.Chave;

        return recursosUsuario;
    }
}