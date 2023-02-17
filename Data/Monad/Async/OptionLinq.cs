using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SMData;
public static class TaskOptionLinq
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Option<TResult>> Select<TOk, TResult>(
       this Task<Option<TOk>> option,
       Func<TOk, TResult> func)
    {
        return option.ContinueWith(t => t.Result.HasValue
            ? Option.Some(func(t.Result.Unwrap()))
            : Option.None);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Option<TResult>> SelectMany<TOk, TResult>(
        this Task<Option<TOk>> option,
        Func<TOk, Task<Option<TResult>>> func)
    {
        return option.ContinueWith(t => t.Result.HasValue
            ? func(t.Result.Unwrap()).Result
            : Option.None);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Option<TResult>> SelectMany<TFirst, TSecond, TResult>(
        this Task<Option<TFirst>> first,
        Func<TFirst, Task<Option<TSecond>>> getSecond,
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
                    return Option.Some(getResult(vfirst, second.Result.Unwrap()));
                }
                else
                {
                    return Option.None;
                }
            }
            return Option.None;
        });
    }
}
