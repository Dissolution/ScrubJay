namespace ScrubJay.Enums;

/// <summary>
/// Extensions on generic instances constrained to <see langword="struct"/> and <see langword="enum"/>.
/// </summary>
[PublicAPI]
public static class TEnumInstanceExtensions
{
    extension<TEnum>(TEnum @enum)
        where TEnum : struct, Enum
    {
        /// <summary>
        /// Is this <typeparamref name="TEnum"/> <see langword="enum"/> the default instance?
        /// </summary>
        public bool IsDefaultInstance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                Emit.Ldarg(nameof(@enum));
                Emit.Ldc_I4_0();
                Emit.Ceq();
                return Return<bool>();
            }
        }

        /// <summary>
        /// Is this <typeparamref name="TEnum"/> <see langword="enum"/> a non-zero power-of-two?<br/>
        /// <i>thus likely a single flag value</i>
        /// </summary>
        public bool IsPowerOfTwo
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                // (x & (x-1)) == 0
                Emit.Ldarg(nameof(@enum));
                Emit.Ldc_I4_1();
                Emit.Sub();
                Emit.Ldarg(nameof(@enum));
                Emit.And();
                Emit.Ldc_I4_0();
                Emit.Ceq();
                return Return<bool>();
            }
        }

        /// <summary>
        /// Get the number of set bit flags in this <typeparamref name="TEnum"/> <see langword="enum"/>.
        /// </summary>
        public int FlagCount
        {
            #if NET6_0_OR_GREATER
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => BitOperations.PopCount(@enum.AsU64());
#else
            get
            {
                ulong value = @enum.AsU64();
                const ulong c1 = 0x_55555555_55555555ul;
                const ulong c2 = 0x_33333333_33333333ul;
                const ulong c3 = 0x_0F0F0F0F_0F0F0F0Ful;
                const ulong c4 = 0x_01010101_01010101ul;

                value -= (value >> 1) & c1;
                value = (value & c2) + ((value >> 2) & c2);
                value = (((value + (value >> 4)) & c3) * c4) >> 56;

                return (int)value;
                
}
            #endif
        }

#region Convert / To / As
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte AsU8()
        {
            Emit.Ldarg_0();
            Emit.Conv_U1();
            return Return<byte>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public sbyte AsI8()
        {
            Emit.Ldarg_0();
            Emit.Conv_I1();
            return Return<sbyte>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ushort AsU16()
        {
            Emit.Ldarg_0();
            Emit.Conv_U2();
            return Return<ushort>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public short AsI16()
        {
            Emit.Ldarg_0();
            Emit.Conv_I2();
            return Return<short>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint AsU32()
        {
            Emit.Ldarg_0();
            Emit.Conv_U4();
            return Return<uint>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int AsI32()
        {
            Emit.Ldarg_0();
            Emit.Conv_I4();
            return Return<int>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong AsU64()
        {
            Emit.Ldarg_0();
            Emit.Conv_U8();
            return Return<ulong>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long AsI64()
        {
            Emit.Ldarg_0();
            Emit.Conv_I8();
            return Return<long>();
        }
#endregion /Convert

#region HasFlags
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasFlag(TEnum flag)
        {
            return (@enum & flag) == flag;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasFlags(TEnum flag)
        {
            return (@enum & flag) == flag;
        }

#region HasAllFlags
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasAllFlags(TEnum flag1)
        {
            return (@enum & flag1) == flag1;
        }

        public bool HasAllFlags(TEnum flag1, TEnum flag2)
        {
            TEnum combined = flag1 | flag2;
            return (@enum & combined) == combined;
        }

        public bool HasAllFlags(TEnum flag1, TEnum flag2, TEnum flag3)
        {
            TEnum combined = flag1 | flag2 | flag3;
            return (@enum & combined) == combined;
        }

        public bool HasAllFlags(TEnum flag1, TEnum flag2, TEnum flag3, TEnum flag4)
        {
            TEnum combined = flag1 | flag2 | flag3 | flag4;
            return (@enum & combined) == combined;
        }

        public bool HasAllFlags(TEnum flag1, TEnum flag2, TEnum flag3, TEnum flag4, TEnum flag5)
        {
            TEnum combined = flag1 | flag2 | flag3 | flag4 | flag5;
            return (@enum & combined) == combined;
        }

        public bool HasAllFlags(params ReadOnlySpan<TEnum> flags)
        {
            TEnum combined = default;
            foreach (var flag in flags)
            {
                combined |= flag;
            }
            return (@enum & combined) == combined;
        }
#endregion /HasAllFlags

#region HasAnyFlags
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasAnyFlags(TEnum flag1)
        {
            return (@enum & flag1) != default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasAnyFlags(TEnum flag1, TEnum flag2)
        {
            return (@enum & (flag1 | flag2)) != default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasAnyFlags(TEnum flag1, TEnum flag2, TEnum flag3)
        {
            return (@enum & (flag1 | flag2 | flag3)) != default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasAnyFlags(TEnum flag1, TEnum flag2, TEnum flag3, TEnum flag4)
        {
            return (@enum & (flag1 | flag2 | flag3 | flag4)) != default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasAnyFlags(TEnum flag1, TEnum flag2, TEnum flag3, TEnum flag4, TEnum flag5)
        {
            return (@enum & (flag1 | flag2 | flag3 | flag4 | flag5)) != default;
        }

        public bool HasAnyFlags(params ReadOnlySpan<TEnum> flags)
        {
            TEnum combined = default;
            foreach (var flag in flags)
            {
                combined |= flag;
            }
            return (@enum & combined) != default;
        }
#endregion /HasAnyFlags
#endregion /HasFlags

#region With
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TEnum WithFlag(TEnum flag) => @enum | flag;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TEnum WithoutFlag(TEnum flag) => @enum & ~flag;
#endregion

#region Equals
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(TEnum other)
        {
            return EnumHelper<TEnum>.Equals(@enum, other);
        }
#endregion /Equals

#region CompareTo
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(TEnum other)
        {
            return EnumHelper<TEnum>.Compare(@enum, other);
        }
#endregion /CompareTo

        
        public string? GetName()
        {
            throw new NotImplementedException();
        }
        
        public string Format(string? format = null)
        {
            throw new NotImplementedException();
        }

        public Result<T> TryConvertTo<T>()
        {
            throw new NotImplementedException();
        }
        
    }

    extension<TEnum>(ref TEnum @enum)
        where TEnum : struct, Enum
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddFlag(TEnum flag)
        {
            @enum |= flag;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveFlag(TEnum flag)
        {
            @enum &= ~flag;
        }
    }
}