using System.Runtime.CompilerServices;

namespace SMResultTypes;
public static class Either
{
    public static Either<TErr, TOk> Ok<TErr, TOk>(TOk ok)   => new Either<TErr, TOk>(ok);
    public static Either<TErr, TOk> Err<TErr, TOk>(TErr ok) => new Either<TErr, TOk>(ok);
    

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueTask<Either<TErr, TOk>> OkAsync<TErr, TOk>(TOk value)
    {
        return new(Ok<TErr, TOk>(value));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueTask<Either<TErr, TOk>> ErrAsync<TErr, TOk>(TErr value)
    {
        return new(Err<TErr, TOk>(value));
    }
}