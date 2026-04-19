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

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int GetXorHashCode(TEnum @enum)
    {
        // we have to account for another possible 32 bits of input data
        // always convert to an u64 first to stabilize behavior

        // load the lower 32 bits onto the stack
        Emit.Ldarg(nameof(@enum));
        Emit.Conv_U8();
        Emit.Conv_I4();

        // load the upper 32 bits onto the stack
        Emit.Ldarg(nameof(@enum));
        Emit.Conv_U8();
        Emit.Ldc_I4(32);
        Emit.Shr();
        Emit.Conv_I4();

        // xor and return
        Emit.Xor();
        return Return<int>();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetHashCode(TEnum @enum)
    {
        if (CanFitInI32)
        {
            Emit.Ldarg(nameof(@enum));
            Emit.Conv_I4();
            return Return<int>();
        }
        else
        {
            return GetXorHashCode(@enum);
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int CompareUnsigned(TEnum left, TEnum right)
    {
        // if left < right return -1
        Emit.Ldarg(nameof(left));
        Emit.Ldarg(nameof(right));
        Emit.Clt_Un();
        Emit.Brtrue("lessThan");

        // if left > right return 1
        Emit.Ldarg(nameof(left));
        Emit.Ldarg(nameof(right));
        Emit.Cgt_Un();
        Emit.Brtrue("greaterThan");

        // else return 0
        Emit.Ldc_I4_0();
        Emit.Ret();

        MarkLabel("lessThan");
        Emit.Ldc_I4_M1();
        Emit.Ret();

        MarkLabel("greaterThan");
        Emit.Ldc_I4_1();
        Emit.Ret();
        throw Unreachable();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Compare(TEnum left, TEnum right)
    {
        if (IsSigned)
        {
            // if left < right return -1
            Emit.Ldarg(nameof(left));
            Emit.Ldarg(nameof(right));
            Emit.Clt();
            Emit.Brtrue("lessThan");

            // if left > right return 1
            Emit.Ldarg(nameof(left));
            Emit.Ldarg(nameof(right));
            Emit.Cgt();
            Emit.Brtrue("greaterThan");

            // else return 0
            Emit.Ldc_I4_0();
            Emit.Ret();

            MarkLabel("lessThan");
            Emit.Ldc_I4_M1();
            Emit.Ret();

            MarkLabel("greaterThan");
            Emit.Ldc_I4_1();
            Emit.Ret();
            throw Unreachable();
        }
        else
        {
            return CompareUnsigned(left, right);
        }
    }
}