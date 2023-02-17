using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SMData;
public static class TaskEitherLinq
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Either<TErr, TResult>> Select<TErr, TOk, TResult>(
       this Task<Either<TErr, TOk>> either,
       Func<TOk, TResult> func)
    {
        return either.ContinueWith(t => t.Result.HasValue
            ? Either.Ok<TErr, TResult>(func(t.Result.Unwrap()))
            : Either.Err<TErr, TResult>(t.Result.UnwrapError()));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Either<TErr, TResult>> SelectMany<TErr, TOk, TResult>(
        this Task<Either<TErr, TOk>> either,
        Func<TOk, Task<Either<TErr, TResult>>> func)
    {
        return either.ContinueWith(t => t.Result.HasValue
            ? func(t.Result.Unwrap()).Result
            : Either.Err<TErr, TResult>(t.Result.UnwrapError()));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Either<TError, TResult>> SelectMany<TFirst, TSecond, TError, TResult>(
        this Task<Either<TError, TFirst>> first,
        Func<TFirst, Task<Either<TError, TSecond>>> getSecond,
        Func<TFirst, TSecond, TResult> getResult)
    {
        return first.ContinueWith(t =>
        {
            if (t.Result.HasValue)
            {
                var vfirst = t.Result.Unwrap();
                var second = getSecond(vfirst);
                if (second.Result.HasValue)
                {
                    return Either.Ok<TError, TResult>(getResult(vfirst, second.Result.Unwrap()));
                }
                else
                {
                    return Either.Err<TError, TResult>(second.Result.UnwrapError());
                }
            }
            return Either.Err<TError, TResult>(first.Result.UnwrapError());
        });
    }
}
