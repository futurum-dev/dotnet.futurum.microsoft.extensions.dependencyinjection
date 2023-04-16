using Futurum.Microsoft.Extensions.DependencyInjection.Generator.Core;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Generator;

public sealed class StartableDatum : IEquatable<StartableDatum>
{
    public StartableDatum(string className,
                          string methodName,
                          bool isStatic)
    {
        ClassName = className;
        MethodName = methodName;
        IsStatic = isStatic;
    }

    public string ClassName { get; }

    public string MethodName { get; }

    public bool IsStatic { get; }

    public bool Equals(StartableDatum other)
    {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;

        return ClassName == other.ClassName
               && MethodName == other.MethodName
               && IsStatic == other.IsStatic;
    }

    public override bool Equals(object obj) =>
        obj is StartableDatum startableRegistration && Equals(startableRegistration);

    public override int GetHashCode() =>
        HashCode.Combine(ClassName, MethodName, IsStatic);

    public static bool operator ==(StartableDatum left, StartableDatum right) =>
        Equals(left, right);

    public static bool operator !=(StartableDatum left, StartableDatum right) =>
        !Equals(left, right);
}