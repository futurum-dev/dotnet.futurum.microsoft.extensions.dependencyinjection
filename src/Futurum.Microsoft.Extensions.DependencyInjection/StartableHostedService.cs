using Microsoft.Extensions.Hosting;

namespace Futurum.Microsoft.Extensions.DependencyInjection;

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