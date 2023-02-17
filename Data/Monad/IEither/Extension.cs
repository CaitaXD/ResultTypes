using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
namespace SMData;
public static class IEitherExtention
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TErr? GetErrorOrDefault<TErr, TOk>(this IEither<TErr, TOk> IEither) =>
        IEither.HasError
            ? IEither.UnwrapError()
            : default;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TOk? GetValueOrDefualt<Err, TOk>(this IEither<Err, TOk> IEither, TOk? defaultValue) =>
       IEither.HasValue
           ? IEither.Unwrap()
           : defaultValue;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryGetValue<TErr, TOk>(
    this IEither<TErr, TOk> IEither,
    [NotNullWhen(true)] out TOk? value)
    {
        value = IEither.GetValueOrDefualt();
        return IEither.HasValue;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryGetValue<TErr, TOk>(
        this IEither<TErr, TOk> IEither,
        [NotNullWhen(true)] out TOk? value,
        [NotNullWhen(false)] out TErr? error)
    {
        value = IEither.GetValueOrDefualt();
        error = IEither.GetErrorOrDefault();
        return IEither.HasValue;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static R Match<TErr, TOk, R>(
        this IEither<TErr, TOk> IEither,
        Func<TErr, R> Err,
        Func<TOk, R> Ok)
    {
        return IEither.HasValue
            ? Ok(IEither.Unwrap())
            : Err(IEither.UnwrapError());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Match<TErr, TOk>(
        this IEither<TErr, TOk> IEither,
        Action<TErr> Err,
        Action<TOk> Ok)
    {
        if (IEither.HasValue)
            Ok(IEither.Unwrap());
        else
            Err(IEither.UnwrapError());
    }
}
