namespace Futurum.Microsoft.Extensions.DependencyInjection.Sample;

public static class AutomaticStaticModule
{
    [RegisterAsDependencyInjectionModule]
    public static void Load(IServiceCollection services)
    {
    }
}