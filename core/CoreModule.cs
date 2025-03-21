public class CoreModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterGeneric(typeof(EntityValidation<,>))
            .As(typeof(IEntityValidation<,>))
            .InstancePerDependency();

        builder.RegisterGeneric(typeof(SolicitacaoAfretamentoEditarStatusValidation<>))
            .As(typeof(ISolicitacaoAfretamentoEditarStatusValidation<>))
            .InstancePerDependency();

        builder.RegisterGeneric(typeof(ProcessoOfertaNegociacaoEditarStatusValidation<>))
            .As(typeof(IProcessoOfertaNegociacaoEditarStatusValidation<>))
            .InstancePerDependency();
    }
}