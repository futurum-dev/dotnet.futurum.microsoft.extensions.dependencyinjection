namespace Futurum.Microsoft.Extensions.DependencyInjection.Sample;

public class AutomaticInstanceStartable
{
    [RegisterAsDependencyInjectionStartable]
    public Task Start()
    {
        return Task.CompletedTask;
    }
}