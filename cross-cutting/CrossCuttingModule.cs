internal class CrossCuttingModule : Module
{
    public bool IsProduction { get; set; }

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<ConfigureCookieAuthenticationOptions>()
            .As<IPostConfigureOptions<CookieAuthenticationOptions>>()
            .SingleInstance();

        if (IsProduction)
        {
            builder.RegisterType<SecurityService>()
            .As<ISecurityService>()
            .SingleInstance();            
        }
        else
        {
            builder.RegisterType<SecurityServiceTest>()
            .As<ISecurityService>()
            .SingleInstance();
        }
    }
}