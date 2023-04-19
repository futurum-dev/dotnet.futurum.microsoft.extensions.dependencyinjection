namespace Futurum.Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Register as a transient in dependency injection, as the only interface it implements
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class RegisterAsTransientAttribute : RegisterAsAttribute
{
}

public static class RegisterAsTransient
{
    /// <summary>
    /// Register as a transient in dependency injection, as self only
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class AsSelfAttribute : RegisterAsAttribute
    {
    }

    /// <summary>
    /// Register as a transient in dependency injection, as specified type
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class AsAttribute<TService> : RegisterAsAttribute
    {
    }

    /// <summary>
    /// Register as a transient in dependency injection, as implemented interfaces
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class AsImplementedInterfacesAttribute : RegisterAsAttribute
    {
    }

    /// <summary>
    /// Register as a transient in dependency injection, as self and implemented interfaces
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class AsImplementedInterfacesAndSelfAttribute : RegisterAsAttribute
    {
    }

    /// <summary>
    /// Register as a scoped in dependency injection, as open generic
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class AsOpenGenericAttribute : RegisterAsAttribute
    {
        public Type? ServiceType { get; set; }
        
        public Type? ImplementationType { get; set; }
    }
}