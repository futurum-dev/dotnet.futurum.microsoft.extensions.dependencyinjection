using Futurum.Microsoft.Extensions.DependencyInjection.Generator.Core;

using Microsoft.CodeAnalysis;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Generator;

public sealed class RegistrationContext : IEquatable<RegistrationContext>
{
    public RegistrationContext(IEnumerable<Diagnostic>? diagnostics = null,
                               IEnumerable<RegistrationDatum>? registrationData = null)
    {
        Diagnostics = new EquatableArray<Diagnostic>(diagnostics);
        RegistrationData = new EquatableArray<RegistrationDatum>(registrationData);
    }

    public EquatableArray<Diagnostic> Diagnostics { get; }

    public EquatableArray<RegistrationDatum> RegistrationData { get; }

    public bool Equals(RegistrationContext other)
    {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;

        return Diagnostics.Equals(other.Diagnostics)
               && RegistrationData.Equals(other.RegistrationData);
    }

    public override bool Equals(object obj) =>
        obj is RegistrationContext registrationContext && Equals(registrationContext);

    public override int GetHashCode() =>
        HashCode.Combine(Diagnostics, RegistrationData);

    public static bool operator ==(RegistrationContext left, RegistrationContext right) =>
        Equals(left, right);

    public static bool operator !=(RegistrationContext left, RegistrationContext right) =>
        !Equals(left, right);
}