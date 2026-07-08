namespace ScrubJay.Text;

[PublicAPI]
public static class TextHelper
{
    internal static unsafe class Unsafe
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo(char* source, char* dest, uint charCount)
        {
            Emit.Ldarg(nameof(dest));
            Emit.Ldarg(nameof(source));
            Emit.Ldarg(nameof(charCount));
            Emit.Sizeof<char>();
            Emit.Mul();
            Emit.Unaligned(0x1);
            Emit.Cpblk();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo(ref readonly char source, ref char dest, uint charCount)
        {
            Emit.Ldarg(nameof(dest));
            Emit.Ldarg(nameof(source));
            Emit.Ldarg(nameof(charCount));
            Emit.Sizeof<char>();
            Emit.Mul();
            Emit.Cpblk();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Copy(ReadOnlySpan<char> source, Span<char> dest, int charCount)
        {
            CopyTo(
                ref MemoryMarshal.GetReference(source),
                ref MemoryMarshal.GetReference(dest),
                (uint)charCount);
        }
    }
}