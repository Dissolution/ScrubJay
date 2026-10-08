#pragma warning disable IDE0060

namespace ScrubJay.Text;

[PublicAPI]
public static unsafe class UnsafeExtensions
{
    extension(Unsafe)
    {
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