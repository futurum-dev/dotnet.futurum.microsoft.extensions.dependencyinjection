namespace Futurum.Microsoft.Extensions.DependencyInjection.Generator.Sample;

public class InstanceStartable
{
    [RegisterAsDependencyInjectionStartable]
    public Task StartAsync()
    {
        return Task.CompletedTask;
    }
}

public static class StaticStartable
{
    [RegisterAsDependencyInjectionStartable]
    public static Task StartAsync()
    {
        return Task.CompletedTask;
    }
}
