using Futurum.Microsoft.Extensions.DependencyInjection.Generator.Core;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Generator;

public sealed class RegistrationDatum : IEquatable<RegistrationDatum>
{
    public RegistrationDatum(string serviceType,
                             string implementationType,
                             RegistrationLifetime lifetime,
                             DuplicateRegistrationStrategy duplicateRegistrationStrategy,
                             string debugServiceType,
                             string debugImplementationType)
    {
        ServiceType = serviceType;
        ImplementationType = implementationType;
        Lifetime = lifetime;
        DuplicateRegistrationStrategy = duplicateRegistrationStrategy;
        DebugServiceType = debugServiceType;
        DebugImplementationType = debugImplementationType;
    }

    public string ServiceType { get; }

    public string ImplementationType { get; }

    public RegistrationLifetime Lifetime { get; }

    public DuplicateRegistrationStrategy DuplicateRegistrationStrategy { get; }

    public string DebugServiceType { get; }

    public string DebugImplementationType { get; }

    public bool Equals(RegistrationDatum other)
    {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;

        return Lifetime == other.Lifetime
               && ImplementationType == other.ImplementationType
               && ServiceType.Equals(other.ServiceType)
               && DuplicateRegistrationStrategy == other.DuplicateRegistrationStrategy
               && DebugServiceType == other.DebugServiceType
               && DebugImplementationType == other.DebugImplementationType;
    }

    public override bool Equals(object obj) =>
        obj is RegistrationDatum serviceRegistration && Equals(serviceRegistration);

    public override int GetHashCode() =>
        HashCode.Combine(Lifetime,
                         ImplementationType,
                         ServiceType,
                         DuplicateRegistrationStrategy,
                         DebugServiceType,
                         DebugImplementationType);

    public static bool operator ==(RegistrationDatum left, RegistrationDatum right) =>
        Equals(left, right);

    public static bool operator !=(RegistrationDatum left, RegistrationDatum right) =>
        !Equals(left, right);
}