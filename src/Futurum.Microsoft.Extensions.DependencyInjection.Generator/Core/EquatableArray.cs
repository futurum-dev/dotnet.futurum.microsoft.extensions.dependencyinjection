using System.Collections;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Generator.Core;

public readonly struct EquatableArray<T> : IEquatable<EquatableArray<T>>, IEnumerable<T>
    where T : IEquatable<T>
{
    public static readonly EquatableArray<T> Empty = new(Array.Empty<T>());

    private readonly T[] _array;

    public EquatableArray(T[] array)
    {
        _array = array;
    }

    public EquatableArray(IEnumerable<T>? array)
    {
        array ??= Enumerable.Empty<T>();
        _array = array.ToArray();
    }

    public bool Equals(EquatableArray<T> array) =>
        AsSpan().SequenceEqual(array.AsSpan());

    public override bool Equals(object obj) =>
        obj is EquatableArray<T> array && Equals(array);

    public override int GetHashCode()
    {
        if (_array is null)
            return 0;

        HashCode hashCode = default;

        foreach (var item in _array)
            hashCode.Add(item);

        return hashCode.ToHashCode();
    }

    private ReadOnlySpan<T> AsSpan() =>
        _array.AsSpan();

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        IEnumerable<T> array = _array ?? Array.Empty<T>();
        return array.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        IEnumerable<T> array = _array ?? Array.Empty<T>();
        return array.GetEnumerator();
    }

    public int Count =>
        _array?.Length ?? 0;

    public static bool operator ==(EquatableArray<T> left, EquatableArray<T> right) =>
        left.Equals(right);

    public static bool operator !=(EquatableArray<T> left, EquatableArray<T> right) =>
        !left.Equals(right);
}