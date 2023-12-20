using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Generator.Sample;

public class InstanceModule
{
    [RegisterAsDependencyInjectionModule]
    public void Register(IServiceCollection services)
    {
        services.TryAddTransient<IModuleService, ModuleService>();
    }
}

public static class StaticModule
{
    [RegisterAsDependencyInjectionModule]
    public static void Register(IServiceCollection services)
    {
        services.TryAddTransient<IModuleService, ModuleService>();
    }
}

public interface IModuleService
{
}

public class ModuleService : IModuleService
{
}
