using Futurum.Microsoft.Extensions.DependencyInjection.Generator.Core;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Generator;

public sealed class RegistrationDatum : IEquatable<RegistrationDatum>
{
    public RegistrationDatum(IEnumerable<string> serviceTypes,
                             string implementationType,
                             RegistrationLifetime lifetime,
                             DuplicateRegistrationStrategy duplicateRegistrationStrategy,
                             InterfaceRegistrationStrategy interfaceRegistration)
    {
        ServiceTypes = new EquatableArray<string>(serviceTypes);
        ImplementationType = implementationType;
        Lifetime = lifetime;
        DuplicateRegistrationStrategy = duplicateRegistrationStrategy;
        InterfaceRegistration = interfaceRegistration;
    }

    public EquatableArray<string> ServiceTypes { get; }

    public string ImplementationType { get; }

    public RegistrationLifetime Lifetime { get; }

    public DuplicateRegistrationStrategy DuplicateRegistrationStrategy { get; }

    public InterfaceRegistrationStrategy InterfaceRegistration { get; }

    public bool Equals(RegistrationDatum other)
    {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;

        return Lifetime == other.Lifetime
               && ImplementationType == other.ImplementationType
               && ServiceTypes.Equals(other.ServiceTypes)
               && DuplicateRegistrationStrategy == other.DuplicateRegistrationStrategy
               && InterfaceRegistration == other.InterfaceRegistration;
    }

    public override bool Equals(object obj) =>
        obj is RegistrationDatum serviceRegistration && Equals(serviceRegistration);

    public override int GetHashCode() =>
        HashCode.Combine(Lifetime,
                         ImplementationType,
                         ServiceTypes,
                         DuplicateRegistrationStrategy,
                         InterfaceRegistration);

    public static bool operator ==(RegistrationDatum left, RegistrationDatum right) =>
        Equals(left, right);

    public static bool operator !=(RegistrationDatum left, RegistrationDatum right) =>
        !Equals(left, right);
}