using Microsoft.Extensions.DependencyInjection;

namespace Futurum.Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Interface for ALL <see cref="Microsoft.Extensions.DependencyInjection"/> modules
/// <remarks>A module allows you to break up registration into logical units i.e. modules</remarks>
/// </summary>
public interface IModule
{
    void Load(IServiceCollection services);
}

/// <summary>
/// Extension methods for <see cref="Microsoft.Extensions.DependencyInjection"/> modules
/// </summary>
public static class ModuleExtensions
{
    /// <summary>
    /// Register a <see cref="Microsoft.Extensions.DependencyInjection"/> module instance
    /// </summary>
    public static IServiceCollection RegisterModule<T>(this IServiceCollection services, T module)
        where T : IModule
    {
        module.Load(services);

        return services;
    }

    /// <summary>
    /// Register a <see cref="Microsoft.Extensions.DependencyInjection"/> module instance
    /// </summary>
    public static IServiceCollection RegisterModule<T>(this IServiceCollection services)
        where T : IModule, new()
    {
        var module = new T();
        
        services.RegisterModule(module);

        return services;
    }
}