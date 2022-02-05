using Microsoft.Extensions.DependencyInjection;

namespace Futurum.Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Interface for ALL <see cref="Microsoft.Extensions.DependencyInjection"/> startables
/// <remarks>A startable is resolved at the start of the application lifecycle and is a place to perform actions as soon as the DependencyInjection container is built.</remarks>
/// </summary>
public interface IStartable
{
    void Start();
}

public static class StartableExtensions
{
    /// <summary>
    /// Adds a <see cref="IStartable"/> that is resolved through DependencyInjection
    /// <remarks>A startable is resolved at the start of the application lifecycle and is a place to perform actions as soon as the DependencyInjection container is built.</remarks>
    /// </summary>
    public static IServiceCollection AddStartable<T>(this IServiceCollection services)
        where T : class, IStartable
    {
        services.AddSingleton<IStartable, T>();

        return services;
    }
    
    /// <summary>
    /// Adds a <see cref="IStartable"/>
    /// <remarks>A startable is resolved at the start of the application lifecycle and is a place to perform actions as soon as the DependencyInjection container is built.</remarks>
    /// </summary>
    public static IServiceCollection AddStartable<T>(this IServiceCollection services, T startable)
        where T : class, IStartable
    {
        services.AddSingleton<IStartable>(startable);

        return services;
    }
    
    /// <summary>
    /// Creates a ServiceProvider containing services from the provided IServiceCollection and starts all <see cref="IStartable"/>'s
    /// <remarks>A startable is resolved at the start of the application lifecycle and is a place to perform actions as soon as the DependencyInjection container is built.</remarks>
    /// </summary>
    public static ServiceProvider BuildServiceProviderWithStartables(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();

        var startables = serviceProvider.GetServices<IStartable>();
        
        foreach (var startable in startables)
        {
            startable.Start();
        }

        return serviceProvider;
    }
}