namespace Futurum.Microsoft.Extensions.DependencyInjection.Sample;

public static class AutomaticStaticStartable
{
    [RegisterAsDependencyInjectionStartable]
    public static Task<int> Start()
    {
        return Task.FromResult(1);
    }
}