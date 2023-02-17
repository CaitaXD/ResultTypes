using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
namespace SMData;
public static class IOptionExtention
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T? GetValueOrDefualt<T>(this IOption<T> option) =>
        option.HasValue
            ? option.Unwrap()
            : default;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T? GetValueOrDefualt<T>(this IOption<T> option, T? defaultValue) =>
        option.HasValue
            ? option.Unwrap()
            : defaultValue;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryGetValue<T>(this IOption<T> option, [NotNullWhen(true)] out T? value)
    {
        value = option.GetValueOrDefualt();
        return option.HasValue;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static R Match<T, R>(this IOption<T> option, Func<T, R> Some, Func<R> None)
    {
        return option.HasValue 
            ? Some(option.Unwrap())
            : None();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Match<T>(this IOption<T> option, Action<T> Some, Action None)
    {
        if (option.HasValue)
            Some(option.Unwrap());
        else
            None();
    }
    
    public static IEnumerator<T> GetEnumerator<T>(this IOption<T> option)
    {
        if (option.HasValue) yield return option.Unwrap(); else yield break;
    }
    public static IEnumerable<T> AsEnumerable<T>(this IOption<T> option)
    {
        using var e = option.GetEnumerator();
        while (e.MoveNext())
            yield return e.Current;
    }
}