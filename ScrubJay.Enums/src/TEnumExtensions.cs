using ScrubJay.Functional;

namespace ScrubJay.Enums;

/// <summary>
/// Static extensions on generic types constrained to <see langword="struct"/> and <see langword="enum"/>.
/// </summary>
[PublicAPI]
public static class TEnumExtensions
{
    extension<TEnum>(TEnum)
        where TEnum : struct, Enum
    {
#region Operators
        // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/enums#206-enum-values-and-operations




#region Bitwise
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TEnum operator ~(TEnum @enum)
        {
            Emit.Ldarg(nameof(@enum));
            Emit.Not();
            return Return<TEnum>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TEnum operator &(TEnum left, TEnum right)
        {
            Emit.Ldarg(nameof(left));
            Emit.Ldarg(nameof(right));
            Emit.And();
            return Return<TEnum>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TEnum operator |(TEnum left, TEnum right)
        {
            Emit.Ldarg(nameof(left));
            Emit.Ldarg(nameof(right));
            Emit.Or();
            return Return<TEnum>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TEnum operator ^(TEnum left, TEnum right)
        {
            Emit.Ldarg(nameof(left));
            Emit.Ldarg(nameof(right));
            Emit.Xor();
            return Return<TEnum>();
        }
#endregion /Bitwise

#region Equality
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(TEnum left, TEnum right)
        {
            Emit.Ldarg(nameof(left));
            Emit.Ldarg(nameof(right));
            Emit.Ceq();
            return Return<bool>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(TEnum left, TEnum right)
        {
            Emit.Ldarg(nameof(left));
            Emit.Ldarg(nameof(right));
            Emit.Ceq();
            Emit.Ldc_I4_0();
            Emit.Ceq();
            return Return<bool>();
        }
#endregion /Equality

#region Comparision
        public static bool operator <(TEnum left, TEnum right)
        {
            Emit.Ldarg(nameof(left));
            Emit.Ldarg(nameof(right));
            if (EnumHelper<TEnum>.IsSigned)
            {
                Emit.Clt();
            }
            else
            {
                Emit.Clt_Un();
            }
            return Return<bool>();
        }

        public static bool operator <=(TEnum left, TEnum right)
        {
            Emit.Ldarg(nameof(left));
            Emit.Ldarg(nameof(right));
            if (EnumHelper<TEnum>.IsSigned)
            {
                Emit.Cgt();
            }
            else
            {
                Emit.Cgt_Un();
            }
            Emit.Ldc_I4_0();
            Emit.Ceq();
            return Return<bool>();
        }

        public static bool operator >(TEnum left, TEnum right)
        {
            Emit.Ldarg(nameof(left));
            Emit.Ldarg(nameof(right));
            if (EnumHelper<TEnum>.IsSigned)
            {
                Emit.Cgt();
            }
            else
            {
                Emit.Cgt_Un();
            }
            return Return<bool>();
        }

        public static bool operator >=(TEnum left, TEnum right)
        {
            Emit.Ldarg(nameof(left));
            Emit.Ldarg(nameof(right));
            if (EnumHelper<TEnum>.IsSigned)
            {
                Emit.Clt();
            }
            else
            {
                Emit.Clt_Un();
            }
            Emit.Ldc_I4_0();
            Emit.Ceq();
            return Return<bool>();
        }
#endregion /Comparision
#endregion /Operators


        public static Type UnderlyingType
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => EnumHelper<TEnum>.UnderlyingType;
        }

        public static bool IsSigned
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => EnumHelper<TEnum>.IsSigned;
        }

        public static TEnum From(ulong u64)
        {
            Emit.Ldarg(nameof(u64));
            return Return<TEnum>();
        }
        
        public static TEnum From(long i64)
        {
            Emit.Ldarg(nameof(i64));
            return Return<TEnum>();
        }
        
        
        public static Result<TEnum> TryParse(scoped text text, bool ignoreCase = true, bool includeAttributes = true)
        {
            throw new NotImplementedException();
        }

        public static Result<TEnum> TryParse(string? str, bool ignoreCase = true, bool includeAttributes = true)
        {
            throw new NotImplementedException();
        }

        public static Result<TEnum> TryParse(long i64)
        {
            throw new NotImplementedException();
        }

        public static Result<TEnum> TryParse(ulong u64)
        {
            throw new NotImplementedException();
        }

        public static Result<TEnum> TryParse(
            object? obj,
            bool ignoreCase = true,
            bool useAttributes = true)
        {
            throw new NotImplementedException();
        }
    }
}