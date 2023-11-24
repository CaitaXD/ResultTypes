namespace SMResultTypes;

public interface IOption
{
    bool HasValue { get; }
}

public interface IOption<out T> : IOption
{
    T? Ok { get; }
    T Unwrap();
    T this[int index] { get; }
    int Count { get; }
}