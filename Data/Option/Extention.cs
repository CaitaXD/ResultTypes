using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using static System.Runtime.CompilerServices.MethodImplOptions;

namespace SMResultTypes;

[PublicAPI]
public static partial class OptionExtensions
{
    [MethodImpl(AggressiveInlining)]
    public static T? GetValueOrDefault<T>(this Option<T> option) =>
        option.HasValue
            ? option.Unwrap()
            : default;

    [MethodImpl(AggressiveInlining)]
    public static T? GetValueOrDefault<T>(this Option<T> option, T? defaultValue) =>
        option.HasValue
            ? option.Unwrap()
            : defaultValue;

    [MethodImpl(AggressiveInlining)]
    public static bool TryGetValue<T>(this Option<T> option, [NotNullWhen(true)] out T? value)
    {
        value = option.GetValueOrDefault();
        return option.HasValue;
    }

    [MethodImpl(AggressiveInlining)]
    public static Option<TR> Map<T, TR>(this Option<T> option, Func<T, TR> map)
    {
        return option.HasValue
            ? new Option<TR>(map(option.Unwrap()))
            : new Option<TR>();
    }

    [MethodImpl(AggressiveInlining)]
    public static Option<TR> MapOption<T, TR>(this Option<T> option, Func<T, Option<TR>> map)
    {
        return option.HasValue
            ? map(option.Unwrap())
            : Option.None;
    }

    [MethodImpl(AggressiveInlining)]
    public static Option<TR> Map<T, TR>(this Option<T> option, TR value)
    {
        return option.HasValue
            ? Option.Some(value)
            : Option.None;
    }

    [MethodImpl(AggressiveInlining)]
    public static Option<TR> MapOption<T, TR>(this Option<T> option, Option<TR> value)
    {
        return option.HasValue
            ? value
            : Option.None;
    }

    [MethodImpl(AggressiveInlining)]
    public static Option<(TResult, TOk)> AppendResult<TOk, TResult>(
        this Option<TOk> option,
        Option<TResult> then)
    {
        return option.HasValue switch {
            true when then.HasValue => (then.Unwrap(), option.Unwrap()),
            _ => Option.None,
        };
    }

    [MethodImpl(AggressiveInlining)]
    public static Option<(TResult, TOk)> AppendResult<TOk, TResult>(
        this Option<TOk> option,
        Func<TOk, Option<TResult>> func)
    {
        var then = option.MapOption(func);
        return option.HasValue switch {
            true when then.HasValue => (then.Unwrap(), option.Unwrap()),
            _ => Option.None,
        };
    }

    [MethodImpl(AggressiveInlining)]
    public static Option<TOk> Fallback<TOk>(this Option<TOk> option, Option<TOk> fallback)
    {
        return !option.HasValue
            ? fallback
            : option;
    }

    [MethodImpl(AggressiveInlining)]
    public static Option<TOk> Fallback<TOk>(
        this Option<TOk> option,
        Func<TOk, Option<TOk>> fallback)
    {
        if (!option.HasValue) {
            return fallback(option.Unwrap());
        }

        return option;
    }

    [MethodImpl(AggressiveInlining)]
    public static bool All<TOk>(this IEnumerable<Option<TOk>> values) => values.All(option => option.HasValue);

    [MethodImpl(AggressiveInlining)]
    public static bool Any<TOk>(this IEnumerable<Option<TOk>> values) => values.Any(option => option.HasValue);

    [MethodImpl(AggressiveInlining)]
    public static Option<TOk> FirstOrNone<TOk>(this IEnumerable<Option<TOk>> values) =>
        values.FirstOrDefault(option => option.HasValue);

    [MethodImpl(AggressiveInlining)]
    public static bool All<TOk>(this Span<Option<TOk>> span)
    {
        if (span.IsEmpty) return false;
        foreach (var option in span) {
            if (!option.HasValue) {
                return false;
            }
        }

        return true;
    }

    [MethodImpl(AggressiveInlining)]
    public static bool Any<TOk>(this Span<Option<TOk>> span)
    {
        if (span.IsEmpty) return false;
        foreach (var option in span) {
            if (option.HasValue) {
                return true;
            }
        }

        return false;
    }

    [MethodImpl(AggressiveInlining)]
    public static Option<TOk> FirstOrNone<TOk>(this Span<Option<TOk>> span)
    {
        if (span.IsEmpty) return Option.None;
        foreach (var option in span) {
            if (option.HasValue) {
                return option;
            }
        }

        return Option.None;
    }

    [MethodImpl(AggressiveInlining)]
    public static async ValueTask<T?> GetValueOrDefaultAsync<T>(this ValueTask<Option<T>> optionAsync,
        T? defaultValue = default)
    {
        var option = await optionAsync.ConfigureAwait(false);
        return option.HasValue
            ? option.Unwrap()
            : defaultValue;
    }

    [MethodImpl(AggressiveInlining)]
    public static async ValueTask<T?> GetValueOrDefaultAsync<T>(this Task<Option<T>> optionAsync,
        T? defaultValue = default)
    {
        var option = await optionAsync.ConfigureAwait(false);
        return option.HasValue
            ? option.Unwrap()
            : defaultValue;
    }
}