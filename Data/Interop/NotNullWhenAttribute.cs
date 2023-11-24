// ReSharper disable once UnusedParameter.Local

#if NETSTANDARD2_0
namespace SMResultTypes;

public class NotNullWhenAttribute : Attribute
{
    public NotNullWhenAttribute(bool _)
    {
    }
}
#endif