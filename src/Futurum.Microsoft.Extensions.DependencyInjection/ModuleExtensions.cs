using Microsoft.Extensions.DependencyInjection;

namespace Futurum.Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for <see cref="Microsoft.Extensions.DependencyInjection"/> modules
/// </summary>
public static class ModuleExtensions
{
    /// <summary>
    /// Register a <see cref="Microsoft.Extensions.DependencyInjection"/> module instance
    /// </summary>
    public static IServiceCollection AddModule<T>(this IServiceCollection services, T module)
        where T : IModule
    {
        module.Load(services);

        return services;
    }

    /// <summary>
    /// Register a <see cref="Microsoft.Extensions.DependencyInjection"/> module instance
    /// </summary>
    public static IServiceCollection AddModule<T>(this IServiceCollection services)
        where T : IModule, new()
    {
        var module = new T();

        services.AddModule(module);

        return services;
    }
}