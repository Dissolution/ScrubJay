// ReSharper disable EntityNameCapturedOnly.Global

namespace ScrubJay.Text;

[PublicAPI]
public static class TextHelper
{
    internal static unsafe class Unsafe
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyCharacters(char* source, char* dest, uint charCount)
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
        public static void CopyCharacters(ref readonly char source, ref char dest, uint charCount)
        {
            Emit.Ldarg(nameof(dest));
            Emit.Ldarg(nameof(source));
            Emit.Ldarg(nameof(charCount));
            Emit.Sizeof<char>();
            Emit.Mul();
            Emit.Cpblk();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyCharacters(scoped ReadOnlySpan<char> source, scoped Span<char> dest, int charCount)
        {
            CopyCharacters(
                ref MemoryMarshal.GetReference(source),
                ref MemoryMarshal.GetReference(dest),
                (uint)charCount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InitCharacters(char* charPtr, uint charCount)
        {
            Emit.Ldarg(nameof(charPtr));
            Emit.Ldc_I4_0();
            Emit.Ldarg(nameof(charCount));
            Emit.Sizeof<char>();
            Emit.Mul();
            Emit.Initblk();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InitCharacters(ref char firstChar, uint charCount)
        {
            Emit.Ldarg(nameof(firstChar));
            Emit.Ldc_I4_0();
            Emit.Ldarg(nameof(charCount));
            Emit.Sizeof<char>();
            Emit.Mul();
            Emit.Initblk();
        }
    }

#region Clear
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clear(scoped Span<char> characters)
    {
        Unsafe.InitCharacters(ref MemoryMarshal.GetReference(characters), (uint)characters.Length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clear(char[]? chars)
    {
        if (chars is not null)
        {
            unsafe
            {
                fixed (char* ptr = chars)
                {
                    Unsafe.InitCharacters(ptr, (uint)chars.Length);
                }
            }
        }
    }
#endregion
#region (Try)CopyTo
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryCopyTo(scoped ReadOnlySpan<char> source, scoped Span<char> dest)
    {
        int len = source.Length;
        if (len <= dest.Length)
        {
            Unsafe.CopyCharacters(source, dest, len);
            return true;
        }
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryCopyTo(string source, scoped Span<char> dest)
    {
        int len = source.Length;
        if (len <= dest.Length)
        {
            Unsafe.CopyCharacters(source, dest, len);
            return true;
        }
        return false;
    }
#endregion

#region SelfCopy
    public static void SelfCopy(char[] array, int sourceIndex, int destIndex, int count)
    {
#if DEBUG
        Debug.Assert(array is not null);
        int arrayLength = array!.Length;

        Debug.Assert(arrayLength > 0);
        Debug.Assert(count > 0);

        Debug.Assert(sourceIndex >= 0 && sourceIndex <= arrayLength);
        int sourceEnd = sourceIndex + count;
        Debug.Assert(sourceEnd >= 0 && sourceEnd <= arrayLength);

        Debug.Assert(destIndex >= 0 && destIndex <= arrayLength);
        int destEnd = destIndex + count;
        Debug.Assert(destEnd >= 0 && destEnd <= arrayLength);
#endif

        ref char src = ref array[sourceIndex];
        ref char dst = ref array[destIndex];
        CopyTo(ref src, ref dst, count);
    }

    public static void SelfCopy(char[] chars, Range source, int destStart)
    {
        Debug.Assert(chars is not null);
        int len = chars!.Length;
        Debug.Assert(len > 0);

        int sourceStart = source.Start.GetOffset(len);
        Debug.Assert(sourceStart >= 0 && sourceStart <= len);

        int sourceEnd = source.End.GetOffset(len);
        Debug.Assert(sourceEnd >= 0 && sourceEnd <= len);

        int sourceLength = sourceEnd - sourceStart;
        Debug.Assert(sourceLength >= 0 && sourceLength <= len);

        Debug.Assert(destStart >= 0 && destStart <= len);

        int destEnd = destStart + sourceLength;
        Debug.Assert(destEnd >= 0 && destEnd <= len);

        int endLength = destEnd - destStart;
        Debug.Assert(endLength >= 0 && endLength <= len);

        Debug.Assert(endLength >= sourceLength);

        ref char src = ref chars[sourceStart];
        ref char dst = ref chars[destStart];
        CopyTo(ref src, ref dst, sourceLength);
    }

    public static void SelfCopy(char[] chars, Range source, Index dest)
    {
        Debug.Assert(chars is not null);
        int len = chars!.Length;
        Debug.Assert(len > 0);

        int sourceStart = source.Start.GetOffset(len);
        Debug.Assert(sourceStart >= 0 && sourceStart <= len);

        int sourceEnd = source.End.GetOffset(len);
        Debug.Assert(sourceEnd >= 0 && sourceEnd <= len);

        int sourceLength = sourceEnd - sourceStart;
        Debug.Assert(sourceLength >= 0 && sourceLength <= len);

        int destStart = dest.GetOffset(len);
        Debug.Assert(destStart >= 0 && destStart <= len);

        int destEnd = destStart + sourceLength;
        Debug.Assert(destEnd >= 0 && destEnd <= len);

        int endLength = destEnd - destStart;
        Debug.Assert(endLength >= 0 && endLength <= len);

        Debug.Assert(endLength >= sourceLength);

        ref char src = ref chars[sourceStart];
        ref char dst = ref chars[destStart];
        CopyTo(ref src, ref dst, sourceLength);
    }

    public static void SelfCopy(char[] chars, Range source, Range dest)
    {
        Debug.Assert(chars is not null);
        int len = chars!.Length;
        Debug.Assert(len > 0);

        int sourceStart = source.Start.GetOffset(len);
        Debug.Assert(sourceStart >= 0 && sourceStart <= len);

        int sourceEnd = source.End.GetOffset(len);
        Debug.Assert(sourceEnd >= 0 && sourceEnd <= len);

        int sourceLength = sourceEnd - sourceStart;
        Debug.Assert(sourceLength >= 0 && sourceLength <= len);

        int destStart = dest.Start.GetOffset(len);
        Debug.Assert(destStart >= 0 && destStart <= len);

        int destEnd = dest.End.GetOffset(len);
        Debug.Assert(destEnd >= 0 && destEnd <= len);

        int endLength = destEnd - destStart;
        Debug.Assert(endLength >= 0 && endLength <= len);

        Debug.Assert(endLength >= sourceLength);

        ref char src = ref chars[sourceStart];
        ref char dst = ref chars[destStart];
        CopyTo(ref src, ref dst, sourceLength);
    }
#endregion
}