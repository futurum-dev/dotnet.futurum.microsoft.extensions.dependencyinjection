using ILogger = Serilog.ILogger;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Sample;

public class ManualStartable : IStartable
{
    private readonly ILogger _logger;

    public ManualStartable(ILogger logger)
    {
        _logger = logger;
    }

    public async Task StartAsync()
    {
        _logger.Information($"{nameof(ManualStartable)} started");
    }
}