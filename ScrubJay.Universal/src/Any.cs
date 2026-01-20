namespace ScrubJay.Universal;

/// <summary>
/// A static utility class for working with <b>any</b> generic value,<br/>
/// including ones with the <c>allows ref struct</c> anti-constraint.
/// </summary>
[PublicAPI]
public static partial class Any { }

#if NET9_0_OR_GREATER
[PublicAPI]
internal static partial class MethodCache<T>
    where T : allows ref struct { }
#endif