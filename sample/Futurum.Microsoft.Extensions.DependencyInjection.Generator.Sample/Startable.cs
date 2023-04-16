namespace Futurum.Microsoft.Extensions.DependencyInjection.Generator.Sample;

public class InstanceStartable
{
    [RegisterAsDependencyInjectionStartable]
    public void Register()
    {
    }
}

public static class StaticStartable
{
    [RegisterAsDependencyInjectionStartable]
    public static void Register()
    {
    }
}