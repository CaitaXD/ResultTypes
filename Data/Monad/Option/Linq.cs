using System.Runtime.CompilerServices;

namespace SMData;
public static class OptionLinq
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TResult> Select<TOk, TResult>(
    this Option<TOk> option,
    Func<TOk, TResult> func)
    {
        return option.HasValue
            ? Option.Some(func(option.Unwrap()))
            : new Option<TResult>();
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TResult> SelectMany<TOk, TResult>(
        this Option<TOk> option,
        Func<TOk, Option<TResult>> func)
    {
        return option.HasValue
            ? func(option.Unwrap())
            : new Option<TResult>();
    } 
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TResult> SelectMany<TFirst, TSecond, TResult>(
        this Option<TFirst> first,
        Func<TFirst, Option<TSecond>> getSecond,
        Func<TFirst, TSecond, TResult> getResult)
    {
        if (first.HasValue)
        {
            var vfirst = first.Unwrap();
            var second = getSecond(vfirst);
            if (second.HasValue)
            {
                return Option.Some(getResult(vfirst, second.Unwrap()));
            }
            else
            {
                return new Option<TResult>();
            }
        }
        return new Option<TResult>();
    }
}
