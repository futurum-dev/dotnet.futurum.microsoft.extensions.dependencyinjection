namespace Futurum.Microsoft.Extensions.DependencyInjection.Generator.Sample;

public class InstanceStartable
{
    [RegisterAsDependencyInjectionStartable]
    public Task Start()
    {
        return Task.CompletedTask;
    }
}

public static class StaticStartable
{
    [RegisterAsDependencyInjectionStartable]
    public static Task Start()
    {
        return Task.CompletedTask;
    }
}