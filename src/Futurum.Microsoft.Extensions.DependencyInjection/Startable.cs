using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

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
        
        services.TryAddSingleton<IHostedService, StartableHostedService>();

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
        
        services.TryAddSingleton<IHostedService, StartableHostedService>();

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

/// <summary>
/// HostedService that resolves all <see cref="IStartable"/>'s and starts them.
/// <remarks>Can't find another way to hook into host lifecycle. Don't take a hard dependency on this mechanism as it may change in future.</remarks>
/// </summary>
internal class StartableHostedService : IHostedService
{
    private readonly IEnumerable<IStartable> _startables;

    public StartableHostedService(IEnumerable<IStartable> startables)
    {
        _startables = startables;
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        foreach (var startable in _startables)
        {
            startable.Start();
        }
        
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) =>
        Task.CompletedTask;
}