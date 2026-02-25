using System.Reflection.Emit;

namespace ScrubJay.Universal;

/// <summary>
/// A static utility class for working with <b>any</b> generic values,<br/>
/// including ones with the <c>allows ref struct</c> anti-constraint.
/// </summary>
/// <see href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/ref-struct"/>
[PublicAPI]
public static partial class Any
{
}

#if NET9_0_OR_GREATER
/// <summary>
/// Internal helper utility for .net9.0+ that dynamically creates and caches delegates that work on <c>ref struct</c>s.
/// </summary>
/// <typeparam name="T"></typeparam>
[PublicAPI]
internal static partial class MethodCache<T>
    where T : allows ref struct
{
    private static void EmitLoadInstance(ILGenerator generator, Type instanceType)
    {
        // stack types
        if (instanceType.IsEnum || instanceType.IsByRef || instanceType.IsByRefLike || instanceType.IsValueType)
        {
            // load a ref to this value
            generator.Emit(
                OpCodes.Ldarga_S,
                0);
        }
        // heap types
        else
        {
            // load the value directly
            generator.Emit(OpCodes.Ldarg_0);
        }
    }
    
    
}
#endif