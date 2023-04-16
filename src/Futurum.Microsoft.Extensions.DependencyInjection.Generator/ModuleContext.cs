using Futurum.Microsoft.Extensions.DependencyInjection.Generator.Core;

using Microsoft.CodeAnalysis;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Generator;

public sealed class ModuleContext : IEquatable<ModuleContext>
{
    public ModuleContext(IEnumerable<Diagnostic>? diagnostics = null,
                         IEnumerable<ModuleDatum>? moduleData = null)
    {
        Diagnostics = new EquatableArray<Diagnostic>(diagnostics);
        ModuleData = new EquatableArray<ModuleDatum>(moduleData);
    }

    public EquatableArray<Diagnostic> Diagnostics { get; }

    public EquatableArray<ModuleDatum> ModuleData { get; }

    public bool Equals(ModuleContext other)
    {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;

        return Diagnostics.Equals(other.Diagnostics)
               && ModuleData.Equals(other.ModuleData);
    }

    public override bool Equals(object obj) =>
        obj is ModuleContext moduleContext && Equals(moduleContext);

    public override int GetHashCode() =>
        HashCode.Combine(Diagnostics, ModuleData);

    public static bool operator ==(ModuleContext left, ModuleContext right) =>
        Equals(left, right);

    public static bool operator !=(ModuleContext left, ModuleContext right) =>
        !Equals(left, right);
}