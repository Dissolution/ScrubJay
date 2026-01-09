#if NET9_0_OR_GREATER

using System.Reflection;
using System.Reflection.Emit;
using BF = System.Reflection.BindingFlags;

namespace ScrubJay.Universal;

partial class Any<T>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNull]
    public static Type? GetType(T? _) => typeof(T);
}

#endif