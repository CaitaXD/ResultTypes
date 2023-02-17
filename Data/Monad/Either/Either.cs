using Google.Protobuf.WellKnownTypes;
using Org.BouncyCastle.Crypto;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SMData;

public readonly record struct Either<TErr, TOk>() : IEither<TErr,TOk>
{
    readonly State _state; 
    internal readonly TErr? _err;
    internal readonly TOk? _ok;
    
    public TOk this[int index] => Unwrap()!;
    public State State { get => _state; }
    public int Count => Convert.ToInt32(HasValue);
    public bool HasValue => State.HasFlag(State.Ok);
    public bool HasError => State.HasFlag(State.Err);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TOk Unwrap() => HasValue
        ? _ok!
        : throw new ArgumentException("Tried to get Value but was Error");
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TOk Unwrap(string messsage) => HasValue
        ? _ok!
        : throw new ArgumentException(messsage);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TErr UnwrapError() => HasError
        ? _err!
        : throw new ArgumentException("Tried to get Error but was Value");
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TErr UnwrapError(string messsage) => HasError
        ? _err!
        : throw new ArgumentException(messsage);

    public Either(TErr err) : this()
    {
        _err = err;
        _ok = default;
        _state = State.Err;
    }
    public Either(TOk ok) : this()
    {
        _err = default;
        _ok = ok;
        _state = State.Ok;
    }

    public static implicit operator bool(Either<TErr, TOk> either) => either.HasValue;
}
