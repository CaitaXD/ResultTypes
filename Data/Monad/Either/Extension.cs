using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
namespace SMData;
public static class EitherExtention
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TErr? GetErrorOrDefault<TErr, TOk>(this Either<TErr, TOk> either) =>
        either.HasError
            ? either.UnwrapError()
            : default;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TOk? GetValueOrDefualt<Err,TOk>(this Either<Err, TOk> either, TOk? defaultValue) =>
       either.HasValue
           ? either.Unwrap()
           : defaultValue;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryGetValue<TErr, TOk>(
    this Either<TErr, TOk> either,
    [NotNullWhen(true)] out TOk? value)
    {
        value = either.GetValueOrDefualt();
        return either.HasValue;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryGetValue<TErr,TOk>(
        this Either<TErr,TOk> either,
        [NotNullWhen(true)] out TOk? value, 
        [NotNullWhen(false)] out TErr? error)
    {
        value = either.GetValueOrDefualt();
        error = either.GetErrorOrDefault();
        return either.HasValue;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static R Match<TErr, TOk, R>(
        this Either<TErr, TOk> either, 
        Func<TErr, R> Err, 
        Func<TOk, R> Ok)
    {
        return either.HasValue
            ? Ok(either.Unwrap())
            : Err(either.UnwrapError());
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Match<TErr, TOk>(
        this Either<TErr, TOk> either,
        Action<TErr> Err,
        Action<TOk> Ok)
    {
        if (either.HasValue)
            Ok(either.Unwrap());
        else
            Err(either.UnwrapError());
    }
}
