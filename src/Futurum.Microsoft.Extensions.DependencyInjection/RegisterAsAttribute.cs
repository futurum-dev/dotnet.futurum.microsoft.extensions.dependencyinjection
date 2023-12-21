using System.Diagnostics.CodeAnalysis;

namespace Futurum.Microsoft.Extensions.DependencyInjection;

[ExcludeFromCodeCoverage(Justification = "No point in testing this. Only used in SourceGenerator.")]
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public abstract class RegisterAsAttribute : Attribute
{
    public DuplicateRegistrationStrategy DuplicateRegistrationStrategy { get; set; }
}
