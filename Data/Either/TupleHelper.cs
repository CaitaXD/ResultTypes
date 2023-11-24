#if NET5_0_OR_GREATER
using System.Runtime.CompilerServices;

namespace SMResultTypes;

public static class TupleHelper<T>
{
    public static List<T> ToList<TTuple>(TTuple tuple, List<T>? acc = null)
        where TTuple : ITuple
    {
        acc ??= new List<T>();
        for (var i = 0; i < tuple.Length; i++) {
            if (tuple[i] is T item) {
                acc.Add(item);
            }
            else if (tuple[i] is ITuple subTuple) {
                _ = ToList(subTuple, acc);
            }
        }
        return acc;
    }
}
#endif