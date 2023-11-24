using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using static System.Runtime.CompilerServices.MethodImplOptions;

namespace SMResultTypes;

[PublicAPI]
public static class EitherExtension
{
    [MethodImpl(AggressiveInlining)]
    public static TErr? GetErrorOrDefault<TErr, TOk>(this Either<TErr, TOk> either, TErr? defaultError = default) =>
        either.HasError
            ? either.UnwrapError()
            : defaultError;

    [MethodImpl(AggressiveInlining)]
    public static TOk? GetValueOrDefault<TErr, TOk>(
        this Either<TErr, TOk> either,
        TOk? defaultValue = default) =>
        either.HasValue
            ? either.Unwrap()
            : defaultValue;

    [MethodImpl(AggressiveInlining)]
    public static bool TryGetValue<TErr, TOk>(
        this Either<TErr, TOk> either,
        [NotNullWhen(true)] out TOk? value)
    {
        value = GetValueOrDefault(either);
        return either.HasValue;
    }

    [MethodImpl(AggressiveInlining)]
    public static bool TryGetValue<TErr, TOk>(
        this Either<TErr, TOk> either,
        [NotNullWhen(true)] out TOk? value,
        [NotNullWhen(false)] out TErr? error)
    {
        value = either.GetValueOrDefault();
        error = either.GetErrorOrDefault();
        return either.HasValue;
    }

    [MethodImpl(AggressiveInlining)]
    public static bool TryGetError<TErr, TOk>(
        this Either<TErr, TOk> either,
        [NotNullWhen(true)] out TErr? error)
    {
        error = either.GetErrorOrDefault();
        return either.HasError;
    }

    [MethodImpl(AggressiveInlining)]
    public static Either<TErr, TResult> Map<TErr, TOk, TResult>(
        this Either<TErr, TOk> either,
        Func<TOk, TResult> func)
    {
        return either.State switch
        {
            State.Ok => func(either.Unwrap()),
            State.Err => either.UnwrapError(),
            State.None => throw new InvalidOperationException("Either is in an invalid state."),
            _ => throw new ArgumentOutOfRangeException(nameof(either))
        };
    }

    [MethodImpl(AggressiveInlining)]
    public static Either<TErr, TResult> Map<TErr, TOk, TResult>(
        this Either<TErr, TOk> either,
        TResult value)
    {
        return either.State switch
        {
            State.Ok => value,
            State.Err => either.UnwrapError(),
            State.None => throw new InvalidOperationException("Either is in an invalid state."),
            _ => throw new ArgumentOutOfRangeException(nameof(either))
        };
    }
    [MethodImpl(AggressiveInlining)]
    public static Either<TErr2, TOk> MapError<TErr, TOk, TErr2>(
        this Either<TErr, TOk> either,
        TErr2 error)
    {
        return either.State switch
        {
            State.Ok => either.Unwrap(),
            State.Err => error,
            State.None => throw new InvalidOperationException("Either is in an invalid state."),
            _ => throw new ArgumentOutOfRangeException(nameof(either))
        };
    }
    [MethodImpl(AggressiveInlining)]
    public static Either<TErr2, TOk> MapError<TErr, TOk, TErr2>(
        this Either<TErr, TOk> either,
        Func<TErr, TErr2> func)
    {
        return either.State switch
        {
            State.Ok => either.Unwrap(),
            State.Err => func(either.UnwrapError()),
            State.None => throw new InvalidOperationException("Either is in an invalid state."),
            _ => throw new ArgumentOutOfRangeException(nameof(either))
        };
    }
    [MethodImpl(AggressiveInlining)]
    public static async ValueTask<Either<TErr2, TOk>> MapErrorAsync<TErr, TOk, TErr2>(
        this ValueTask<Either<TErr, TOk>> either,
        TErr2 error)
    {
        return (await either).State switch
        {
            State.Ok => (await either).Unwrap(),
            State.Err => error,
            State.None => throw new InvalidOperationException("Either is in an invalid state."),
            _ => throw new ArgumentOutOfRangeException(nameof(either))
        };
    }
    [MethodImpl(AggressiveInlining)]
    public static async ValueTask<Either<TErr2, TOk>> MapErrorAsync<TErr, TOk, TErr2>(
        this ValueTask<Either<TErr, TOk>> either,
        Func<TErr, TErr2> func)
    {
        return (await either).State switch
        {
            State.Ok => either.Result.Unwrap(),
            State.Err => func(either.Result.UnwrapError()),
            State.None => throw new InvalidOperationException("Either is in an invalid state."),
            _ => throw new ArgumentOutOfRangeException(nameof(either))
        };
    }
    [MethodImpl(AggressiveInlining)]
    public static Either<TErr, TResult> MapEither<TErr, TOk, TResult>(
        this Either<TErr, TOk> either,
        Func<TOk, Either<TErr, TResult>> func)
    {
        return either.State switch
        {
            State.Ok => func(either.Unwrap()),
            State.Err => either.UnwrapError(),
            State.None => throw new InvalidOperationException("Either is in an invalid state."),
            _ => throw new ArgumentOutOfRangeException(nameof(either))
        };
    }

    [MethodImpl(AggressiveInlining)]
    public static Either<TErr, TResult> MapEither<TErr, TOk, TResult>(
        this Either<TErr, TOk> either,
        Either<TErr, TResult> other)
    {
        return either.State switch
        {
            State.Ok => other,
            State.Err => either.UnwrapError(),
            State.None => throw new InvalidOperationException("Either is in an invalid state."),
            _ => throw new ArgumentOutOfRangeException(nameof(either))
        };
    }

    [MethodImpl(AggressiveInlining)]
    public static async ValueTask<Either<TErr, TResult>> MapEitherAsync<TErr, TOk, TResult>(
    this ValueTask<Either<TErr, TOk>> either,
    ValueTask<Either<TErr, TResult>> other)
    {
        return (await either).State switch
        {
            State.Ok => await other,
            State.Err => (await either).UnwrapError(),
            State.None => throw new InvalidOperationException("Either is in an invalid state."),
            _ => throw new ArgumentOutOfRangeException(nameof(either))
        };
    }

    [MethodImpl(AggressiveInlining)]
    public static async ValueTask<Either<TErr, TResult>> MapEitherAsync<TErr, TOk, TResult>(
    this ValueTask<Either<TErr, TOk>> either,
    Func<TOk, ValueTask<Either<TErr, TResult>>> func)
    {
        return (await either).State switch
        {
            State.Ok => await func(either.Result.Unwrap()),
            State.Err => (await either).UnwrapError(),
            State.None => throw new InvalidOperationException("Either is in an invalid state."),
            _ => throw new ArgumentOutOfRangeException(nameof(either))
        };
    }

    [MethodImpl(AggressiveInlining)]
    public static Either<TErr, (TResult, TOk)> AppendResult<TErr, TOk, TResult>(
        this Either<TErr, TOk> either,
        Either<TErr, TResult> then)
    {
        return either.State switch
        {
            State.Ok when then.HasValue => (then.Unwrap(), either.Unwrap()),
            State.Ok => then.UnwrapError(),
            State.Err => either.UnwrapError(),
            State.None => throw new InvalidOperationException("Either is in an invalid state."),
            _ => throw new ArgumentOutOfRangeException(nameof(either))
        };
    }

    [MethodImpl(AggressiveInlining)]
    public static Either<TErr, (TResult, TOk)> AppendResult<TErr, TOk, TResult>(
        this Either<TErr, TOk> either,
        Func<TOk, Either<TErr, TResult>> func)
    {
        switch (either.State)
        {
        case State.Ok:
        {
            var result = func(either.Unwrap());
            return result.HasValue switch
            {
                true => (result.Unwrap(), either.Unwrap()),
                _ => result.UnwrapError()
            };
        }
        case State.Err: return either.UnwrapError();
        case State.None:
        default:
            throw new InvalidOperationException("Either is in an invalid state.");
        }
    }

    [MethodImpl(AggressiveInlining)]
    public static Either<TErr, TOk> Fallback<TErr, TOk>(this Either<TErr, TOk> either, Either<TErr, TOk> fallback)
    {
        return !either.HasValue
            ? fallback
            : either;
    }

    [MethodImpl(AggressiveInlining)]
    public static Either<TErr, TOk> Fallback<TErr, TOk>(
        this Either<TErr, TOk> either,
        Func<TOk, Either<TErr, TOk>> fallback)
    {
        if (!either.HasValue)
        {
            return fallback(either.Unwrap());
        }

        return either;
    }

    [MethodImpl(AggressiveInlining)]
    public static bool All<TErr, TOk>(this Span<Either<TErr, TOk>> span)
    {
        if (span.IsEmpty) return false;
        foreach (var either in span)
        {
            if (either.HasError)
            {
                return false;
            }
        }

        return true;
    }

    [MethodImpl(AggressiveInlining)]
    public static bool Any<TErr, TOk>(this Span<Either<TErr, TOk>> span)
    {
        if (span.IsEmpty) return false;
        foreach (var either in span)
        {
            if (either.HasValue)
            {
                return true;
            }
        }

        return false;
    }

    [MethodImpl(AggressiveInlining)]
    public static async ValueTask<TOk?> GetValueOrDefaultAsync<TErr, TOk>(
        this ValueTask<Either<TErr, TOk>> eitherAsync,
        TOk? defaultValue = default)
    {
        var either = await eitherAsync.ConfigureAwait(false);
        return either.HasValue ? either.Unwrap() : defaultValue;
    }
    [MethodImpl(AggressiveInlining)]
    public static async ValueTask<TOk?> GetValueOrDefaultAsync<TErr, TOk>(
        this Task<Either<TErr, TOk>> eitherAsync,
        TOk? defaultValue = default)
    {
        var either = await eitherAsync.ConfigureAwait(false);
        return either.HasValue ? either.Unwrap() : defaultValue;
    }
    [MethodImpl(AggressiveInlining)]
    public static async ValueTask<TErr?> GetErrorOrDefaultAsync<TErr, TOk>(
        this ValueTask<Either<TErr, TOk>> eitherAsync,
        TErr? defaultError = default)
    {
        var either = await eitherAsync.ConfigureAwait(false);
        return either.HasValue ? either.UnwrapError() : defaultError;
    }
    [MethodImpl(AggressiveInlining)]
    public static async ValueTask<TErr?> GetErrorOrDefaultAsync<TErr, TOk>(
        this Task<Either<TErr, TOk>> eitherAsync,
        TErr? defaultError = default)
    {
        var either = await eitherAsync.ConfigureAwait(false);
        return either.HasValue ? either.UnwrapError() : defaultError;
    }
}