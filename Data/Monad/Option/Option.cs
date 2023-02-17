using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace SMData;

public readonly struct Option<T> :
    IOption<T>,
    IEquatable<None>,
    IEquatable<IOption<T>>
{
    internal readonly T? _value;
    public bool HasValue { get; init; }
    public int Count => Convert.ToInt32(HasValue);
    public T this[int index] => _value!;
    public Option(T value)
    {
        _value = value;
        HasValue = true;
    }
    public Option()
    {
        _value = default;
        HasValue = false;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Unwrap() => HasValue
      ? _value!
      : throw new ArgumentException("Tried to get Value but was None");
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Unwrap(string messsage) => HasValue
        ? _value!
        : throw new ArgumentException(messsage);

    public override int GetHashCode()
    {
        return HasValue
            ? _value!.GetHashCode()
            : 0;
    }
    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is None ? !HasValue : base.Equals(obj);
    public bool Equals(IOption<T>? other) => other switch
    {
        null => other is null,
        None none => other.Equals(none),
        _ => HasValue
            ? other.Unwrap()!.Equals(_value)
            : HasValue == other.HasValue,
    };
    bool IEquatable<None>.Equals(None _) => HasValue is false;
    public static bool operator ==(Option<T> left, Option<T> right) => left.Equals(right);
    public static bool operator !=(Option<T> left, Option<T> right) => !(left==right);
    public static implicit operator bool(Option<T> option) => option.HasValue;
    public static implicit operator Option<T>(None _) => new();
    public IEnumerator<T> GetEnumerator()
    {
        if (HasValue) yield return _value!; else yield break;
    }
}
