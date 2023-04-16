namespace Futurum.Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Method will be called as a startable for dependency injection
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public sealed class RegisterAsDependencyInjectionStartableAttribute : Attribute
{
}