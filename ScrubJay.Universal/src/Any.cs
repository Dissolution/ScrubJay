namespace ScrubJay.Universal;

[PublicAPI]
public static partial class Any { }

#if NET9_0_OR_GREATER
[PublicAPI]
public static partial class MethodCache<T>
    where T : allows ref struct { }
#endif