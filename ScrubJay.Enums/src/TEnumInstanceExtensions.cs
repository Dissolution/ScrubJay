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
}