
namespace SMData;
public interface IOption
{
    bool HasValue { get; }
}
public interface IOption<out T> : IOption
{
    T Unwrap();
    T this[int index] { get; }
    int Count { get; }
    IEnumerator<T> GetEnumerator()
    {
        if (HasValue) yield return Unwrap(); else yield break;
    }
    IEnumerable<T> AsEnumerable()
    {
        using var e = GetEnumerator();
        while (e.MoveNext())
            yield return e.Current;
    }
}
