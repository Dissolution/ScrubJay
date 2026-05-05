namespace ScrubJay.Enums;

[PublicAPI]
public static class EnumHelper<TEnum>
    where TEnum : struct, Enum
{
    /// <summary>
    /// Do <typeparamref name="TEnum"/> <see langword="enum"/>s have a signed underlying type?
    /// </summary>
    public static readonly bool IsSigned;

    public static readonly Type UnderlyingType;

    public static readonly bool CanFitInU32;
    public static readonly bool CanFitInI32;
    public static readonly bool CanFitInU64;

    public static readonly int Size = Unsafe.SizeOf<TEnum>();
    
    static EnumHelper()
    {
        var type = typeof(TEnum);
        UnderlyingType = type.GetEnumUnderlyingType()!;

        var typeCode = Type.GetTypeCode(UnderlyingType);
        /* Enums can only have underlying types from 5 (sbyte) to 12 (uint64):
         *   SByte = 5,          // Signed 8-bit integer
         *   Byte = 6,           // Unsigned 8-bit integer
         *   Int16 = 7,          // Signed 16-bit integer
         *   UInt16 = 8,         // Unsigned 16-bit integer
         *   Int32 = 9,          // Signed 32-bit integer
         *   UInt32 = 10,        // Unsigned 32-bit integer
         *   Int64 = 11,         // Signed 64-bit integer
         *   UInt64 = 12,        // Unsigned 64-bit integer
         */

        IsSigned = typeCode is TypeCode.SByte or TypeCode.Int16 or TypeCode.Int32 or TypeCode.Int64;
        CanFitInU32 = typeCode == TypeCode.UInt32 || typeCode < TypeCode.Int32;
        CanFitInI32 = typeCode <= TypeCode.Int32;
        CanFitInU64 = typeCode != TypeCode.Int64;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equals(TEnum left, TEnum right)
    {
        Emit.Ldarg(nameof(left));
        Emit.Ldarg(nameof(right));
        Emit.Ceq();
        return Return<bool>();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool NotEquals(TEnum left, TEnum right)
    {
        Emit.Ldarg(nameof(left));
        Emit.Ldarg(nameof(right));
        Emit.Ceq();
        Emit.Ldc_I4_0();
        Emit.Ceq();
        return Return<bool>();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetHashCode(TEnum @enum)
    {
        ulong value = Unsafe.As<TEnum, ulong>(ref @enum);
        return (int)value ^ (int)(value >> 32);
    }

    public static int Compare(TEnum left, TEnum right)
    {
        if (IsSigned)
        {
            long l = Unsafe.As<TEnum, long>(ref left);
            long r = Unsafe.As<TEnum, long>(ref right);
            return (l > r ? 1 : 0) - (l < r ? 1 : 0);
        }
        else
        {
            ulong l = Unsafe.As<TEnum, ulong>(ref left);
            ulong r = Unsafe.As<TEnum, ulong>(ref right);
            return (l > r ? 1 : 0) - (l < r ? 1 : 0);
        }
    }
}