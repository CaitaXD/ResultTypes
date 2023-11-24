namespace SMResultTypes;

[Flags]
public enum State : byte
{
    None = 00,
    Ok   = 01,
    Err  = 10,
}