using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace SMResultTypes;

[PublicAPI]
public readonly record struct Either<TErr, TOk>() : IEither<TErr, TOk>
{
    public TErr? Err { get; init; }
    public TOk?  Ok  { get; init; }

    public TOk this[int index] => Unwrap()!;
    public State State    { get; }
    public int   Count    => Convert.ToInt32(HasValue);
    public bool  HasValue => (State & State.Ok)  != 0;
    public bool  HasError => (State & State.Err) != 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TOk Unwrap() => HasValue
        ? Ok!
        : throw new ArgumentException("Tried to get Value but was Error");
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TOk Unwrap(string message) => HasValue
        ? Ok!
        : throw new ArgumentException(message);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TErr UnwrapError() => HasError
        ? Err!
        : throw new ArgumentException("Tried to get Error but was Value");
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TErr UnwrapError(string message) => HasError
        ? Err!
        : throw new ArgumentException(message);
    public Either(TErr err) : this()
    {
        Err   = err;
        Ok    = default;
        State = State.Err;
    }
    public Either(TOk ok) : this()
    {
        Err   = default;
        Ok    = ok;
        State = State.Ok;
    }
    public override string ToString()
    {
        return State switch {
            State.Ok => $"Ok({Ok})",
            State.Err => $"Err({Err})",
            _ => "None"
        };
    }
    public static implicit operator Either<TErr, TOk>(TErr err) => new(err);
    public static implicit operator Either<TErr, TOk>(TOk ok)   => new(ok);

    public void Deconstruct(out TErr? err, out TOk? ok)
    {
        err = Err;
        ok  = Ok;
    }
    public void Deconstruct(out State state, out TErr? err, out TOk? ok)
    {
        state = State;
        err   = Err;
        ok    = Ok;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<TOk> AsEnumerable()
    {
        if (HasValue) yield return Ok!;
    }
}