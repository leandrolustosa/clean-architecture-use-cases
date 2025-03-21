# Projeto com arquitetura limpa, utilizando use cases

Nesse projeto eu fui responsável pela arquitetura e desenvolvimento de toda a estrutura core do código. Eu tomei a decisão de utilizar a arquitetura limpa (Clean Architecture) e Use Case Pattern, pois sabia que tínhamos um tempo muito pequeno para o desenvolvimento, de um projeto muito grande, o modelo de dados possuia mais de 70 tabelas. Para isso idealizei uma arquitetura onde os desenvolvedores, teriam que seguir um passo a passo para implementar cada Caso de Uso.

Acredito que o grande ganho que tivemos nesse projeto, foi de fato a velocidade com que construímos, muito ajudados pela arquitetura e pelo baixo índice de retrabalho, isso foi decorrente do Use Case Pattern. Esse padrão exige que cada caso de uso seja implementado em uma classe e que cada classe apenas tenha um método público. Dessa forma uma vez que o caso de uso era entregue, poucas vezes precisávamos revisá-lo, a não quando havia alterações nas regras de negócio, mas que ocorria poucas vezes.

O código do projeto não está em sua plenitude aqui, eu fiz um recorte para apresentar um pouco da arquitetura como um todo.

Agora vou explicar um pouco do código, separado por camada:

## Camada web-api

Nos `Controllers` relacionados a entidades do negócio (`ProcessoOfertaNegociacaoController`), o objetivo é que só houvesse implementação ligada ao próprio negócio, abstraindo ao máximo a parte técnica nas classes bases, aqui nesse caso `AppBaseController` e `GenericBaseController`.

Nesse contexto uma classe que é central para abstrair a parte técnica é o `UseCaseFacade` que se utiliza do pattern `Facade` para "esconder" a lógica de resolução das dependências necessárias para a resolução de cada Caso de Uso. Mas eu explicarei melhor essa classe na camada `application`.

Embaixo observamos a simples implementação de todo o CRUD da entidade `ProcessoOfertaNegociacao` pelo seu controller correspondente:

```c#
[HttpGet]
public async Task<IActionResult> Listar(DataSourceLoadOptions options)
{
    return await List<ProcessoOfertaNegociacaoDto>(options);
}

[HttpPut]
public async Task<IActionResult> Editar([FromBody] ProcessoOfertaNegociacaoEditarDto dto)
{
    return await Update<IProcessoOfertaNegociacaoEditarUseCase, ProcessoOfertaNegociacaoEditarDto>(dto);
}

[HttpPost]
public async Task<IActionResult> Incluir([FromBody] ProcessoOfertaNegociacaoIncluirDto dto)
{
    return await Insert<IProcessoOfertaNegociacaoIncluirUseCase, ProcessoOfertaNegociacaoIncluirDto>(dto);
}

[HttpDelete]
public async Task<IActionResult> Excluir([FromQuery] ProcessoOfertaNegociacaoExcluirDto dto)
{
    return await Remove<IProcessoOfertaNegociacaoExcluirUseCase, ProcessoOfertaNegociacaoExcluirDto>(dto);
}

[HttpGet]
public async Task<IActionResult> Obter([FromQuery] ProcessoOfertaNegociacaoListarDto dto)
{
    return await Get(dto);
}
```

E quando o caso de uso não era uma operação do CRUD, contávamos com o `UseCaseFacade` para criar um caso de uso específico, conforme pode ser visto abaixo:

```c#
[HttpPost]
public async Task<IActionResult> CalcularValorOferta([FromBody] CalculoValorOfertaDto dto)
{
    var result = await Facade.ExecuteAsync<ICalcularValorOfertaUseCase, CalculoValorOfertaDto>(dto);

    if (result.Sucesso)
    {
        return Ok(result);
    }

    return CreateErrorResponse(result);
}
[HttpPost]
public async Task<IActionResult> IniciarNegociacao([FromBody] ProcessoOfertaIniciarNegociacaoDto dto)
{
    var result = await Facade.ExecuteAuditableAsync<IProcessoOfertaIniciarNegociacaoUseCase, ProcessoOfertaIniciarNegociacaoDto, ListaOfertasRankingDto>(dto, LoggedUser);

    if (result.Sucesso)
    {
        return Ok(result);
    }

    return CreateErrorResponse(result);
}
[HttpPost]
public async Task<IActionResult> PermiteInicioNegociacao([FromBody] ListaOfertasRankingDto dto)
{
    var result = await Facade.ExecuteAsync<IProcessoOfertaValidaInicioNegociacaoUseCase, ListaOfertasRankingDto>(dto);

    return Ok(result);

}
```

## Camada application

Quando trabalhamos no desenvolvimento de uma aplicação, temos que dar uma mensagem de sucesso ou de falha cada vez que executamos uma ação, ou caso de uso. Nesse contexto, surge uma decisão a ser tomada, trabalhar com `Exception` a cada erro, falha, ou validação fora do esperado, ou trabalhar com o Result Pattern. Nesse projeto eu tomei a decisão de trabalhar com o Result Pattern, eu acredito que com esse padrão, tenhamos mais controle do que está retornando para o usuário final, também há muitos estudos que defendem que gerar `Exceptions` ao longo do código de forma propositada, gera um overhead grande de performance, mas nem tudo são flores, quando trabalhamos com esse padrão, sempre precisamos tratar se vamos apresentar um `Sucesso` ou um `Erro` para o usuário.

Na pasta `use-cases` temos onde ocorre toda a orquestração do sistema, como eu disse anteriormente a classe `UseCaseFacade` abstrai a lógica de resolução das dependências que cada caso de uso necessita, utilizando para isso outra importante classe nesse cenário a `UseCaseManager`, para gerir o container de dependências, utilizamos a biblioteca `Autofac` que possui recursos poderosos para entregar as dependências certas, para cenários complexos.

Veja abaixo um exemplo de resolução de dependências:

```c#
public IAuditableUseCase<TDto, TDtoReturn> ResolveAuditableUseCase<TUseCase, TDto, TDtoReturn>(TDto dto, User user)
    where TUseCase : IAuditableUseCase<TDto, TDtoReturn>
    where TDto : AuditableEntityDto
    where TDtoReturn : EntityDto
{
    return container.Resolve<TUseCase>(new TypedParameter(typeof(TDto), dto), new TypedParameter(typeof(User), user));
}
```

Em um cenário de execução se eu tinha um caso de uso `ProcessoOfertaNegociacaoIncluirUseCase` conforme a classe abaixo:

```c#
public class ProcessoOfertaNegociacaoIncluirUseCase : AuditableUseCase<ProcessoOfertaNegociacaoIncluirDto>, IProcessoOfertaNegociacaoIncluirUseCase
{        
    private readonly IProcessoOfertaRepository _ofertaRepository;        
    private readonly IProcessoOfertaNegociacaoIncluirValidation _validation;
    private readonly IUseCaseManager _manager;

    public ProcessoOfertaNegociacaoIncluirUseCase(ProcessoOfertaNegociacaoIncluirDto dto, IProcessoOfertaRepository ofertaRepository, 
        IProcessoOfertaNegociacaoIncluirValidation validation, IUseCaseManager manager, 
        IMapper mapper, IAuditContext auditContext, User user, IUnitOfWork uow) 
        : base(dto, user, auditContext, mapper, uow)
    {
        _ofertaRepository = ofertaRepository;            
        _validation = validation;
        _manager = manager;
    }
}
```

A linha `container.Resolve<TUseCase>(new TypedParameter(typeof(TDto), dto), new TypedParameter(typeof(User), user));` encontra o caso de uso, injeta no seu construtor os parâmetros `dto` e `user` e os demais parâmetros para instanciar a classe são resolvidos automaticamente pelo `Autofac`.

Outro ponto importante desse projeto, é que um dos requisitos requeria que a grande maioria das entidades de negócio precisariam ter auditoria de todas as operações que ocorresem, por isso, vamos observar uma classe base `AuditableUseCase`.

Essa camada também temos as validações de interface, basicamente valida-se se as informações enviadas estão dentro de um padrão esperado, talvez um campo numérico tenha que ser um número maior do que `ZERO` ou um campo texto não pode ter mais do que `255`caracteres e assim por diante.

E por fim a classe `ApplicationModule` registra as dependências dessa camada através da função `RegisterGeneric`, no exemplo abaixo todas as classes que herdam do Use Case x são registradas automaticamente, simplificando muito esse processo.

```c#
builder.RegisterGeneric(typeof(IncluirUseCase<,>))
            .As(typeof(IIncluirUseCase<,>))
            .InstancePerDependency();
```

## Camada core

Possui classes para lidar com as regras de negócio da aplicação, aqui por exemplo encontramos as classes de validação de negócio.

## Camada domain

São classes representando as entidades de negócio, além dos DTO representando as entidades de negócio, que no final são os objetos que retornam para a interface do usuário.

## Camada infrastructure

É a camada de acesso ao banco de dados, aqui eu adotei padrões bem consolidados como `Repository` e `UnitOfWork`, esses patterns além de criarem uma camada de abstração entre as camadas de aplicação e acesso a dados, também abstraem a tecnologia do banco de dados que está por trás da aplicação, curiosamente nesse projeto iniciamos o desenvolvimento com SQL Server, mas o cliente exigiu que a entrega fosse em Oracle, para realizarmos essa virada do ponto de vista da aplicação foi muito simples, com pouquíssimo retrabalho.

Nessa camada também implementamos toda a lógica para obter e salvar todos os dados de auditoria das ações em cada entidade de negócio, toda essa lógica pode ser observada na classe `ApplicationDbContext`.

## Camada cross-cutting

Como o nome já diz essa camada corta todas as demais camadas, então aqui encontraremos classes para implementar o IoC (Inversion of Control) e DI (Dependency Injection), bem como implementação relacionada a segurança.