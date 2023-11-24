namespace SMResultTypes;

public interface IEither<out TErr, out TOk> : IOption<TOk>
{
    bool HasError { get; }
    State State { get; }
    TErr UnwrapError();
}