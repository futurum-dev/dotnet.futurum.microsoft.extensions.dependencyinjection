using Microsoft.Extensions.DependencyInjection;

namespace Futurum.Microsoft.Extensions.DependencyInjection;

public static class ModuleExtensions
{
    public static IServiceCollection RegisterModule<T>(this IServiceCollection services, T module)
        where T : IModule
    {
        module.Load(services);

        return services;
    }

    public static IServiceCollection RegisterModule<T>(this IServiceCollection services)
        where T : IModule, new()
    {
        var module = new T();
        
        services.RegisterModule(module);

        return services;
    }

    public static IServiceCollection RegisterModule<T>(this IServiceCollection services, Func<IServiceCollection,T> factory)
        where T : IModule
    {
        var module = factory(services);
        
        services.RegisterModule(module);

        return services;
    }
}