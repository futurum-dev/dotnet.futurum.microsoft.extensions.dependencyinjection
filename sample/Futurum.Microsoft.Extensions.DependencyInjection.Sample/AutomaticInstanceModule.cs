namespace Futurum.Microsoft.Extensions.DependencyInjection.Sample;

public class AutomaticInstanceModule
{
    [RegisterAsDependencyInjectionModule]
    public void Load(IServiceCollection services)
    {
    }
}