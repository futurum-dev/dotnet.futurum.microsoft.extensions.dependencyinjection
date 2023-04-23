namespace Futurum.Microsoft.Extensions.DependencyInjection.Sample;

public static class AutomaticStaticStartable
{
    [RegisterAsDependencyInjectionStartable]
    public static Task<int> StartAsync()
    {
        return Task.FromResult(1);
    }
}