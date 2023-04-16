namespace Futurum.Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Register as a scoped in dependency injection
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class RegisterAsScopedAttribute : RegisterAsAttribute
{
}