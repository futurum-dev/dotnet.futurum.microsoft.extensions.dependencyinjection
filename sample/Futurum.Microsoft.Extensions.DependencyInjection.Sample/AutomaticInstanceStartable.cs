namespace Futurum.Microsoft.Extensions.DependencyInjection.Sample;

public class AutomaticInstanceStartable
{
    [RegisterAsDependencyInjectionStartable]
    public Task StartAsync()
    {
        return Task.CompletedTask;
    }
}