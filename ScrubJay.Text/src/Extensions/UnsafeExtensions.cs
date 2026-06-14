#pragma warning disable IDE0060

namespace ScrubJay.Text.Extensions;

[PublicAPI]
public static unsafe class UnsafeExtensions
{
    extension(TextHelper.Unsafe)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyBlock<T>(T* destination, T* source, uint itemCount)
            where T : unmanaged
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyBlock(char* destination, char* source, uint characterCount)
        {
            Emit.Ldarg_0();
            Emit.Ldarg_1();
            Emit.Ldarg_2();
            Emit.Sizeof<char>();
            Emit.Mul();
            Emit.Cpblk();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyBlock(ref char destination, ref readonly char source, uint characterCount)
        {
            Emit.Ldarg_0();
            Emit.Ldarg_1();
            Emit.Ldarg_2();
            Emit.Sizeof<char>();
            Emit.Mul();
            Emit.Cpblk();
        }
    }
}