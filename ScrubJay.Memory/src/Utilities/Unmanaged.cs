using InlineIL;

namespace ScrubJay.Memory.Utilities;

[PublicAPI]
public static unsafe class Unmanaged
{
    public static T Clone<T>(ref readonly T value)
        where T : unmanaged
#if NET9_0_OR_GREATER
    , allows ref struct
#endif
    {
        if (sizeof(T) == 1)
        {
            Emit.Ldarg(nameof(value));
            Emit.Unaligned(0x1);
            Emit.Ldind_U1();
            return Return<T>();
        }

        if (sizeof(T) == 2)
        {
            Emit.Ldarg(nameof(value));
            Emit.Unaligned(0x1);
            Emit.Ldind_U2();
            return Return<T>();
        }

        if (sizeof(T) == 4)
        {
            Emit.Ldarg(nameof(value));
            Emit.Unaligned(0x1);
            Emit.Ldind_U4();
            return Return<T>();
        }

        if (sizeof(T) == 8)
        {
            Emit.Ldarg(nameof(value));
            Emit.Unaligned(0x1);
            Emit.Ldind_I8();
            return Return<T>();
        }

        Emit.Ldarg(nameof(value));
        Emit.Unaligned(0x1);
        Emit.Ldobj<T>();
        return Return<T>();
    }

    public static void Reverse<T>(ref T value)
        where T : unmanaged
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        AsBytes(ref value).Reverse();
    }
    
    
    public static ref readonly TOut InAsIn<TIn, TOut>(ref readonly TIn input)
#if NET9_0_OR_GREATER
        where TIn : unmanaged, allows ref struct
        where TOut : unmanaged, allows ref struct
#else
        where TIn : unmanaged
        where TOut : unmanaged
#endif
    {
        Emit.Ldarg(nameof(input));
        return ref ReturnRef<TOut>();
    }
    
    public static ref TOut RefAsRef<TIn, TOut>(ref TIn input)
#if NET9_0_OR_GREATER
        where TIn : unmanaged, allows ref struct
        where TOut : unmanaged, allows ref struct
#else
        where TIn : unmanaged
        where TOut : unmanaged
#endif
    {
        Emit.Ldarg(nameof(input));
        return ref ReturnRef<TOut>();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<byte> AsReadOnlyBytes<T>(ref readonly T value)
        where T : unmanaged
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        Emit.Ldarg(nameof(value)); // treat as void*
        Emit.Sizeof<T>();
        Emit.Newobj(MethodRef.Constructor(typeof(ReadOnlySpan<byte>), [typeof(void*), typeof(int)]));
        Emit.Ret();
        throw Unreachable();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<byte> AsBytes<T>(ref T value)
        where T : unmanaged
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        Emit.Ldarg(nameof(value)); // treat as void*
        Emit.Sizeof<T>();
        Emit.Newobj(MethodRef.Constructor(typeof(Span<byte>), [typeof(void*), typeof(int)]));
        Emit.Ret();
        throw Unreachable();
    }
}