namespace Futurum.Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Register as a transient in dependency injection
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class RegisterAsTransientAttribute : RegisterAsAttribute
{
}

/// <summary>
/// Register as a transient in dependency injection
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class RegisterAsTransientAttribute<TService> : RegisterAsAttribute
{
}