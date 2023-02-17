using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
namespace SMData;
public static class OptionExtention
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T? GetValueOrDefualt<T>(this Option<T> option) =>
        option.HasValue
            ? option.Unwrap()
            : default;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T? GetValueOrDefualt<T>(this Option<T> option, T? defaultValue) =>
        option.HasValue
            ? option.Unwrap()
            : defaultValue;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryGetValue<T>(this Option<T> option, [NotNullWhen(true)] out T? value)
    {
        value = option.GetValueOrDefualt();
        return option.HasValue;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static R Match<T, R>(this Option<T> option, Func<T, R> Some, Func<R> None)
    {
        return option.HasValue
            ? Some(option.Unwrap())
            : None();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Match<T>(this Option<T> option, Action<T> Some, Action None)
    {
        if (option.HasValue)
            Some(option.Unwrap());
        else
            None();
    }
    
    public static IEnumerator<T> GetEnumerator<T>(this Option<T> option)
    {
        if (option.HasValue) yield return option.Unwrap(); else yield break;
    }
    public static IEnumerable<T> AsEnumerable<T>(this Option<T> option)
    {
        using var e = option.GetEnumerator();
        while (e.MoveNext())
            yield return e.Current;
    }
}