using System.Runtime.CompilerServices;
namespace SMData;
public static class IEitherLinq
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEither<TErr, TResult> Select<TErr, TOk, TResult>(
        this IEither<TErr, TOk> either,
        Func<TOk, TResult> func)
    {
        return either.HasValue
            ? Either.Ok<TErr, TResult>(func(either.Unwrap()))
            : Either.Err<TErr, TResult>(either.UnwrapError());
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEither<TErr, TResult> SelectMany<TErr, TOk, TResult>(
        this IEither<TErr, TOk> either,
        Func<TOk, IEither<TErr, TResult>> func)
    {
        return either.HasValue
            ? func(either.Unwrap())
            : Either.Err<TErr, TResult>(either.UnwrapError());
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEither<TError, TResult> SelectMany<TFirst, TSecond, TError, TResult>(
        this IEither<TError, TFirst> first,
        Func<TFirst, IEither<TError, TSecond>> getSecond,
        Func<TFirst, TSecond, TResult> getResult)
    {
        if(first.HasValue)
        {
            var vfirst = first.Unwrap();
            var second = getSecond(vfirst);
            if (second.HasValue)
            {
                return Either.Ok<TError, TResult>(getResult(vfirst, second.Unwrap()));
            }
            else
            {
                return Either.Err<TError, TResult>(second.UnwrapError());
            }
        }
        return Either.Err<TError, TResult>(first.UnwrapError());
    }
}
