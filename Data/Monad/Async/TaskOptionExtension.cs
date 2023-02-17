using SMData;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SMData;
public static class TaskOptionExtension
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<T?> GetValueOrDefualtAsync<T>(this Task<Option<T>> option)
    {
        return option.ContinueWith(t => t.Result.GetValueOrDefualt());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<T?> GetValueOrDefualtAsync<T>(this Task<Option<T>> option, T? defaultValue)
    {
        return option.ContinueWith(t => t.Result.GetValueOrDefualt(defaultValue));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<R> MatchAsync<T, R>(this Task<Option<T>> option, Func<T, R> Some, Func<R> None)
    {
        return option.ContinueWith(t => t.Result.Match(Some, None));
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task Match<T>(this Task<Option<T>> option, Action<T> Some, Action None)
    {
       return option.ContinueWith(t => t.Result.Match(Some, None));
    }
    
    public static async IAsyncEnumerator<T> GetAsyncEnumerator<T>(this Task<Option<T>> option)
    {
        if ((await option).HasValue) yield return option.Result.Unwrap(); else yield break;
    }
    public static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(this Task<Option<T>> option)
    {
        var e = option.GetAsyncEnumerator();
        while (await e.MoveNextAsync())
            yield return e.Current;
    }
}
