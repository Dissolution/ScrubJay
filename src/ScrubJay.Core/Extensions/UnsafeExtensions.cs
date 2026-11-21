#pragma warning disable IDE0060 // Remove unused parameter
using static InlineIL.IL;

namespace ScrubJay.Extensions;

[PublicAPI]
public static unsafe class UnsafeExtensions
{
    extension(Unsafe)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyBlock<T>(T* destination, T* source, uint itemCount)
            where T : unmanaged
#if NET9_0_OR_GREATER
            , allows ref struct
#endif
        {
            Emit.Ldarg_0();
            Emit.Ldarg_1();
            Emit.Ldarg_2();
            Emit.Sizeof<T>();
            Emit.Mul();
            Emit.Cpblk();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyBlock(byte* destination, byte* source, uint byteCount)
        {
            Emit.Ldarg_0();
            Emit.Ldarg_1();
            Emit.Ldarg_2();
            Emit.Cpblk();
        }
    }
}