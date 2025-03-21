public static class ContainerSetup
{
    public static void AutofacInitialization(ContainerBuilder builder, bool isProduction)
    {
        var context = new AssemblyLoadContext("IoC");
                    
        var applicationAssembly = context.LoadFromAssemblyName(new AssemblyName("application"));
        var domainAssembly = context.LoadFromAssemblyName(new AssemblyName("domain"));
        var coreAssembly = context.LoadFromAssemblyName(new AssemblyName("core"));
        var infrastructureAssembly = context.LoadFromAssemblyName(new AssemblyName("infrastructure"));            
        var crossCuttingAssembly = Assembly.GetExecutingAssembly();

        builder.RegisterAssemblyTypes(domainAssembly, applicationAssembly, coreAssembly, infrastructureAssembly, crossCuttingAssembly).AsImplementedInterfaces();

        builder.RegisterAssemblyModules(coreAssembly, applicationAssembly, infrastructureAssembly);

        builder.RegisterModule(new CrossCuttingModule() { 
            IsProduction = isProduction
        });
    }
}