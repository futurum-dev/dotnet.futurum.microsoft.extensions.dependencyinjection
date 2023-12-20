using System.Diagnostics.CodeAnalysis;

namespace Futurum.Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Register as a keyed transient in dependency injection, as the only interface it implements
/// </summary>
[ExcludeFromCodeCoverage(Justification = "No point in testing this. Only used in SourceGenerator.")]
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class RegisterAsKeyedTransientAttribute : RegisterAsKeyedAttribute
{
    public RegisterAsKeyedTransientAttribute(string key) : base(key)
    {
    }
}

public static partial class RegisterAsTransient
{
    /// <summary>
    /// Register as a keyed transient in dependency injection, as self only
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "No point in testing this. Only used in SourceGenerator.")]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class AsKeyedSelfAttribute : RegisterAsKeyedAttribute
    {
        public AsKeyedSelfAttribute(string key) : base(key)
        {
        }
    }

    /// <summary>
    /// Register as a keyed transient in dependency injection, as specified type
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "No point in testing this. Only used in SourceGenerator.")]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class AsKeyedAttribute<TService> : RegisterAsKeyedAttribute
    {
        public AsKeyedAttribute(string key) : base(key)
        {
        }
    }

    /// <summary>
    /// Register as a keyed transient in dependency injection, as implemented interfaces
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "No point in testing this. Only used in SourceGenerator.")]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class AsKeyedImplementedInterfacesAttribute : RegisterAsKeyedAttribute
    {
        public AsKeyedImplementedInterfacesAttribute(string key) : base(key)
        {
        }
    }

    /// <summary>
    /// Register as a keyed transient in dependency injection, as self and implemented interfaces
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "No point in testing this. Only used in SourceGenerator.")]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class AsKeyedImplementedInterfacesAndSelfAttribute : RegisterAsKeyedAttribute
    {
        public AsKeyedImplementedInterfacesAndSelfAttribute(string key) : base(key)
        {
        }
    }

    /// <summary>
    /// Register as a keyed transient in dependency injection, as open generic
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "No point in testing this. Only used in SourceGenerator.")]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class AsKeyedOpenGenericAttribute : RegisterAsKeyedAttribute
    {
        public AsKeyedOpenGenericAttribute(string key) : base(key)
        {
        }

        public Type? ServiceType { get; set; }

        public Type? ImplementationType { get; set; }
    }
}
