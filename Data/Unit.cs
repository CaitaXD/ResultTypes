using JetBrains.Annotations;

namespace SMResultTypes;

[PublicAPI] public record struct Unit
{
    public readonly static Unit Value;
    readonly static        ValueTuple Vt   = ValueTuple.Create();
    public static implicit operator Unit(ValueTuple _) => Value;
    public static implicit operator ValueTuple(Unit _) => Vt;
}