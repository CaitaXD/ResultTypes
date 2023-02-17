using System.Runtime.CompilerServices;
namespace SMData;
public static class EitherLinq
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Either<TErr, TResult> Select<TErr, TOk, TResult>(
        this Either<TErr, TOk> either,
        Func<TOk, TResult> func)
    {
        return either.HasValue
            ? Either.Ok<TErr, TResult>(func(either.Unwrap()))
            : Either.Err<TErr, TResult>(either.UnwrapError());
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Either<TErr, TResult> SelectMany<TErr, TOk, TResult>(
        this Either<TErr, TOk> either,
        Func<TOk, Either<TErr, TResult>> func)
    {
        return either.HasValue
            ? func(either.Unwrap())
            : Either.Err<TErr, TResult>(either.UnwrapError());
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Either<TError, TResult> SelectMany<TFirst, TSecond, TError, TResult>(
        this Either<TError, TFirst> first,
        Func<TFirst, Either<TError, TSecond>> getSecond,
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
