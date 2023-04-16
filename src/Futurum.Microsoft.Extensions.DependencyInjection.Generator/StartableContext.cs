using Futurum.Microsoft.Extensions.DependencyInjection.Generator.Core;

using Microsoft.CodeAnalysis;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Generator;

public sealed class StartableContext : IEquatable<StartableContext>
{
    public StartableContext(IEnumerable<Diagnostic>? diagnostics = null,
                            IEnumerable<StartableDatum>? startableData = null)
    {
        Diagnostics = new EquatableArray<Diagnostic>(diagnostics);
        StartableData = new EquatableArray<StartableDatum>(startableData);
    }

    public EquatableArray<Diagnostic> Diagnostics { get; }

    public EquatableArray<StartableDatum> StartableData { get; }

    public bool Equals(StartableContext other)
    {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;

        return Diagnostics.Equals(other.Diagnostics)
               && StartableData.Equals(other.StartableData);
    }

    public override bool Equals(object obj) =>
        obj is StartableContext startableContext && Equals(startableContext);

    public override int GetHashCode() =>
        HashCode.Combine(Diagnostics, StartableData);

    public static bool operator ==(StartableContext left, StartableContext right) =>
        Equals(left, right);

    public static bool operator !=(StartableContext left, StartableContext right) =>
        !Equals(left, right);
}