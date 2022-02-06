using ILogger = Serilog.ILogger;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Sample;

public class SampleStartable : IStartable
{
    private readonly ILogger _logger;

    public SampleStartable(ILogger logger)
    {
        _logger = logger;
    }
    
    public void Start()
    {
        _logger.Information($"{nameof(SampleStartable)} started");
    }
}