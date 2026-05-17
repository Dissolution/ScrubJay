namespace ScrubJay.Text.Extensions;

[PublicAPI]
public static unsafe class UnsafeExtensions
{
    extension(Unsafe)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyBlock<T>(T* destination, T* source, uint itemCount)
            where T : unmanaged
        {
            IL.Emit.Ldarg_0();
            IL.Emit.Ldarg_1();
            IL.Emit.Ldarg_2();
            IL.Emit.Sizeof<T>();
            IL.Emit.Mul();
            IL.Emit.Cpblk();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyBlock(byte* destination, byte* source, uint byteCount)
        {
            IL.Emit.Ldarg_0();
            IL.Emit.Ldarg_1();
            IL.Emit.Ldarg_2();
            IL.Emit.Cpblk();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyBlock(char* destination, char* source, uint characterCount)
        {
            IL.Emit.Ldarg_0();
            IL.Emit.Ldarg_1();
            IL.Emit.Ldarg_2();
            IL.Emit.Sizeof<char>();
            IL.Emit.Mul();
            IL.Emit.Cpblk();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyBlock(ref char destination, ref readonly char source, uint characterCount)
        {
            IL.Emit.Ldarg_0();
            IL.Emit.Ldarg_1();
            IL.Emit.Ldarg_2();
            IL.Emit.Sizeof<char>();
            IL.Emit.Mul();
            IL.Emit.Cpblk();
        }
    }
}