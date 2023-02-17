using System.Runtime.CompilerServices;

namespace SMData;

public static class IOptionLinq
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IOption<TResult> Select<TOk, TResult>(
    this IOption<TOk> option,
    Func<TOk, TResult> func)
    {
        return option.HasValue
            ? Option.Some(func(option.Unwrap()))
            : new Option<TResult>();
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IOption<TResult> SelectMany<TOk, TResult>(
        this IOption<TOk> option,
        Func<TOk, IOption<TResult>> func)
    {
        return option.HasValue
            ? func(option.Unwrap())
            : new Option<TResult>();
    } 
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IOption<TResult> SelectMany<TFirst, TSecond, TResult>(
        this IOption<TFirst> first,
        Func<TFirst, IOption<TSecond>> getSecond,
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
