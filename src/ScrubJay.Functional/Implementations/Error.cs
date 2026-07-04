using System.ComponentModel;

namespace ScrubJay.Functional.Implementations;

[PublicAPI]
[EditorBrowsable(EditorBrowsableState.Never)]
[StructLayout(LayoutKind.Auto)]
public readonly ref struct Error<T>
#if NET9_0_OR_GREATER
where T : allows ref struct
#endif
{
    internal readonly T _value;

    internal Error(T value)
    {
        _value = value;
    }
}