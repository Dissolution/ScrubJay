namespace ScrubJay.Universal.Extensions;

/// <summary>
/// 
/// </summary>
[PublicAPI]
public static class UniversalExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Is<T>(this object? obj, [MaybeNullWhen(false)] out T value)
    {
        /* The obvious C# code:
         * // if (obj is T)
         * // {
         * //     value = (T)obj;
         * //     return true;
         * // }
         * // else
         * // {
         * //     value = default;
         * //     return false;
         * // }
         *
         * Compiles into this IL:
         * IL_0000: ldarg.0      // obj
         * IL_0001: isinst       !!0/*T* /
         * IL_0006: brfalse.s    IL_0016
         * IL_0008: ldarg.1      // 'value'
         * IL_0009: ldarg.0      // obj
         * IL_000a: unbox.any    !!0/*T* /
         * IL_000f: stobj        !!0/*T* /
         * IL_0014: ldc.i4.1
         * IL_0015: ret
         * IL_0016: ldarg.1      // 'value'
         * IL_0017: initobj      !!0/*T* /
         * IL_001d: ldc.i4.0
         * IL_001e: ret
         *
         * This is not the most performant code, as the result of the `isinst` is only used to compare with `default(T)`
         * Where I can just store the result directly in the value and then check
         */

        Emit.Ldarg(nameof(value));
        Emit.Ldarg(nameof(obj));
        Emit.Isinst<T>();
        Emit.Stind_Ref();
        Emit.Ldarg(nameof(value));
        Emit.Ldind_Ref();
        Emit.Ldnull();
        Emit.Cgt_Un();
        Emit.Ret();
        throw Unreachable();
    }

    extension<T>(Nullable<T> nullable)
        where T : struct
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetValue(out T value)
        {
            value = nullable.GetValueOrDefault();
            return nullable.HasValue;
        }
    }
}
