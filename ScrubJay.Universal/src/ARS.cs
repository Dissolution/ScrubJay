namespace ScrubJay.Universal;

[PublicAPI]
[StructLayout(LayoutKind.Auto, Size = 0)]
public readonly struct AllowsRefStruct<T>
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
;