public class ApplicationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterGeneric(typeof(ProcessoOfertaNegociacaoEditarStatusUseCase<>))
            .As(typeof(IProcessoOfertaNegociacaoEditarStatusUseCase<>))
            .InstancePerDependency();

        builder.RegisterGeneric(typeof(ListarUseCase<,,>))
            .As(typeof(IListarUseCase<,,>))
            .InstancePerDependency();

        builder.RegisterGeneric(typeof(GetSingleUseCase<,>))
            .As(typeof(IGetSingleUseCase<,>))
            .InstancePerDependency();

        builder.RegisterGeneric(typeof(IncluirUseCase<,>))
            .As(typeof(IIncluirUseCase<,>))
            .InstancePerDependency();

        builder.RegisterGeneric(typeof(EditarUseCase<,>))
            .As(typeof(IEditarUseCase<,>))
            .InstancePerDependency();

        builder.RegisterGeneric(typeof(ExcluirUseCase<,>))
            .As(typeof(IExcluirUseCase<,>))
            .InstancePerDependency();

        builder.RegisterGeneric(typeof(LookupUseCase<>))
            .As(typeof(ILookupUseCase<>))
            .InstancePerDependency();

        builder.RegisterGeneric(typeof(LookupQueryUseCase<,>))
            .As(typeof(ILookupQueryUseCase<,>))
            .InstancePerDependency();
    }
}
