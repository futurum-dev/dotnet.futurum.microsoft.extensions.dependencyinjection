namespace Futurum.Microsoft.Extensions.DependencyInjection.Sample;

public static class AutomaticStaticStartable
{
    [RegisterAsDependencyInjectionStartable]
    public static void Start()
    {
        var x = 10;
    }
}