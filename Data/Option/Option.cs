using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace SMResultTypes;

[PublicAPI]
public readonly struct Option<T> :
    IOption<T>,
    IEquatable<None>,
    IEquatable<IOption<T>>
{
    public T? Ok { get; init; }
    public bool HasValue { get; init; }
    public int Count => HasValue ? 1 : 0;

    public T this[int index] => Ok!;

    public Option(T? ok)
    {
        Ok = ok;
        HasValue = ok is not null;
    }

    public Option()
    {
        Ok = default;
        HasValue = false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Unwrap() => HasValue ? Ok! : throw new ArgumentException("Tried to get Value but was None");

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Unwrap(string message) => HasValue ? Ok! : throw new ArgumentException(message);

    public override int GetHashCode()
    {
        return HashCode.Combine(Ok, HasValue);
    }

    public bool Equals(Option<T> other)
    {
        return this switch
        {
            { HasValue: false } when other is { HasValue: false } => true,
            { HasValue: true } when other is { HasValue : true } => EqualityComparer<T?>.Default.Equals(Ok,
                other.Unwrap()),
            _ => false
        };
    }

    public bool Equals(IOption<T>? other)
    {
        return this switch
        {
            { HasValue: false } when other is { HasValue: false } => true,
            { HasValue: true } when other is { HasValue : true } => EqualityComparer<T?>.Default.Equals(Ok,
                other.Unwrap()),
            _ => false
        };
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is Option<T> other && Equals(other);
    }

    bool IEquatable<None>.Equals(None _) => HasValue is false;
    public static implicit operator Option<T>(None _) => new();
    public static implicit operator Option<T>(T value) => new(value);
    public static bool operator ==(Option<T> left, Option<T> right) => left.Equals(right);
    public static bool operator !=(Option<T> left, Option<T> right) => !(left == right);
    public override string ToString() => HasValue ? $"Some({Ok!})" : "None";

    public string ToString(string format, IFormatProvider? formatProvider = null)
    {
        if (HasValue is false || Ok is null) {
            return "None";
        }

        if (Ok is IFormattable formattable) {
            return formattable.ToString(format, formatProvider);
        }

        string formatString = $"{{0:{format}}}";
        return HasValue ? string.Format(formatProvider, formatString, Ok) : "None";
    }

    public IEnumerator<T> GetEnumerator()
    {
        if (HasValue) yield return Ok!;
    }

    public IEnumerable<T> AsEnumerable()
    {
        if (HasValue) yield return Ok!;
    }
}