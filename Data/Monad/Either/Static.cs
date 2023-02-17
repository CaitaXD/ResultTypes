using System.Runtime.CompilerServices;

namespace SMData;
public static class Either
{
    public static Either<TErr, TOk> Ok<TErr, TOk>(TOk ok) => new(ok);
    public static Either<TErr, TOk> Err<TErr, TOk>(TErr ok) => new(ok);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Either<TErr, TOk>> OkAsync<TErr, TOk>(TOk value)
    {
        return Task.FromResult(Either.Ok<TErr, TOk>(value));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Either<TErr, TOk>> ErrAsync<TErr, TOk>(TErr value)
    {
        return Task.FromResult(Either.Err<TErr, TOk>(value));
    }
}