using System.Diagnostics.CodeAnalysis;

namespace Futurum.Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Register as a scoped in dependency injection, as the only interface it implements
/// </summary>
[ExcludeFromCodeCoverage(Justification = "No point in testing this. Only used in SourceGenerator.")]
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class RegisterAsScopedAttribute : RegisterAsAttribute
{
}

public static partial class RegisterAsScoped
{
    /// <summary>
    /// Register as a scoped in dependency injection, as self only
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "No point in testing this. Only used in SourceGenerator.")]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class AsSelfAttribute : RegisterAsAttribute
    {
    }

    /// <summary>
    /// Register as a scoped in dependency injection, as specified type
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "No point in testing this. Only used in SourceGenerator.")]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class AsAttribute<TService> : RegisterAsAttribute
    {
    }

    /// <summary>
    /// Register as a scoped in dependency injection, as implemented interfaces
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "No point in testing this. Only used in SourceGenerator.")]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class AsImplementedInterfacesAttribute : RegisterAsAttribute
    {
    }

    /// <summary>
    /// Register as a scoped in dependency injection, as self and implemented interfaces
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "No point in testing this. Only used in SourceGenerator.")]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class AsImplementedInterfacesAndSelfAttribute : RegisterAsAttribute
    {
    }

    /// <summary>
    /// Register as a scoped in dependency injection, as open generic
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "No point in testing this. Only used in SourceGenerator.")]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class AsOpenGenericAttribute : RegisterAsAttribute
    {
        public Type? ServiceType { get; set; }

        public Type? ImplementationType { get; set; }
    }
}
