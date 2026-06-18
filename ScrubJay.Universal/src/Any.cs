namespace ScrubJay.Universal;

/// <summary>
/// A static utility class for working with <b>any</b> generic value,<br/>
/// including ones with the <c>allows ref struct</c> anti-constraint.
/// </summary>
/// <see href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/ref-struct"/>
[PublicAPI]
[SuppressMessage("ReSharper", "MethodOverloadWithOptionalParameter")]
public static partial class Any;