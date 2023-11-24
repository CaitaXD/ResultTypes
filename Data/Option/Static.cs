using System.Runtime.CompilerServices;

namespace SMResultTypes;

public static partial class Option
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> Some<T>(T? value) => new Option<T>(value);
    public static None None => new None();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task<Option<T>> SomeAsync<T>(T value)
    {
        return Task.FromResult(Some(value));
    }

    public static Task<None> NoneAsync => Task.FromResult(None);

#if NET7_0_OR_GREATER
        public static Option<T> Parse<T>(string s, IFormatProvider? formatProvider = null)
        where T : IParsable<T>
    {
        return T.TryParse(s, formatProvider, out var result) ? result : None;
    }
#endif
}