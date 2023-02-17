using System.Runtime.CompilerServices;

namespace SMData;
public static class Option
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> Some<T>(T value) => new(value);
    public static None None => new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Option<T>> SomeAsync<T>(T value)
    {
        return Task.FromResult(Option.Some(value));
    }

    public static Task<None> NoneAsync => Task.FromResult(Option.None);
}
