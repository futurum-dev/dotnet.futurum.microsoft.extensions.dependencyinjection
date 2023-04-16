namespace Futurum.Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Register as a singleton in dependency injection
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class RegisterAsSingletonAttribute : RegisterAsAttribute
{
}