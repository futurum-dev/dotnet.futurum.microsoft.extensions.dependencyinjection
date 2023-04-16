namespace Futurum.Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Method will be called as a module for dependency injection
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public sealed class RegisterAsDependencyInjectionModuleAttribute : Attribute
{
}