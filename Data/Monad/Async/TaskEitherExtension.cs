using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SMData;
public static class TaskEitherExtension
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<TOk?> GetValueOrDefualtAsync<TErr,TOk>(this Task<Either<TErr, TOk>> option)
    {
        return option.ContinueWith(t => t.Result.GetValueOrDefualt());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<TOk?> GetValueOrDefualtAsync<TErr, TOk>(
        this Task<Either<TErr, TOk>> option, TOk? defaultValue)
    {
        return option.ContinueWith(t => t.Result.GetValueOrDefualt(defaultValue));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<R> MatchAsync<TErr, TOk, R>(
        this Task<Either<TErr, TOk>> option, 
        Func<TErr, R> Err, 
        Func<TOk, R> Ok)
    {
        return option.ContinueWith(t => t.Result.Match(Err, Ok));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task Match<TErr,TOk>(this Task<Either<TErr,TOk>> option, Action<TOk> Some, Action None)
    {
        return option.ContinueWith(t => t.Result.Match(Some, None));
    }
}
