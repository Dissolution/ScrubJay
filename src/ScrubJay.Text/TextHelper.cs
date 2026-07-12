// ReSharper disable EntityNameCapturedOnly.Global

using ScrubJay.Text.Extensions;

namespace ScrubJay.Text;

[PublicAPI]
public static class TextHelper
{
    internal static unsafe class Notsafe
    {
        #region CopyCharacters
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
#endregion
        
        #region InitCharacters
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
        #endregion
    }

#region Clear
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clear(scoped Span<char> characters)
    {
        Notsafe.InitCharacters(ref MemoryMarshal.GetReference(characters), (uint)characters.Length);
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
                    Notsafe.InitCharacters(ptr, (uint)chars.Length);
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
            Notsafe.CopyCharacters(source, dest, len);
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
            Notsafe.CopyCharacters(source, dest, len);
            return true;
        }
        return false;
    }
#endregion
    
    public static bool TryUnboxText(object? obj, out text text)
    {
        if (obj is char)
        {
            ref char ch = ref Unsafe.Unbox<char>(obj);
            text = ch.AsSpan();
            return true;
        }

        if (obj is string str)
        {
            text = str.AsSpan();
            return true;
        }

        if (obj is char[] chars)
        {
            text = chars.AsSpan();
            return true;
        }

        if (obj is ReadOnlyMemory<char> memory)
        {
            text = memory.Span;
            return true;
        }

        text = default;
        return false;
    }
}