using Futurum.Microsoft.Extensions.DependencyInjection.Generator.Core;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Generator;

public sealed class RegistrationDatum : IEquatable<RegistrationDatum>
{
    public RegistrationDatum(string serviceType,
                             string implementationType,
                             RegistrationLifetime lifetime,
                             DuplicateRegistrationStrategy duplicateRegistrationStrategy)
    {
        ServiceType = serviceType;
        ImplementationType = implementationType;
        Lifetime = lifetime;
        DuplicateRegistrationStrategy = duplicateRegistrationStrategy;
    }

    public string ServiceType { get; }

    public string ImplementationType { get; }

    public RegistrationLifetime Lifetime { get; }

    public DuplicateRegistrationStrategy DuplicateRegistrationStrategy { get; }

    public bool Equals(RegistrationDatum other)
    {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;

        return Lifetime == other.Lifetime
               && ImplementationType == other.ImplementationType
               && ServiceType.Equals(other.ServiceType)
               && DuplicateRegistrationStrategy == other.DuplicateRegistrationStrategy;
    }

    public override bool Equals(object obj) =>
        obj is RegistrationDatum serviceRegistration && Equals(serviceRegistration);

    public override int GetHashCode() =>
        HashCode.Combine(Lifetime,
                         ImplementationType,
                         ServiceType,
                         DuplicateRegistrationStrategy);

    public static bool operator ==(RegistrationDatum left, RegistrationDatum right) =>
        Equals(left, right);

    public static bool operator !=(RegistrationDatum left, RegistrationDatum right) =>
        !Equals(left, right);
}