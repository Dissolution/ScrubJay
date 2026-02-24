using static InlineIL.IL;


namespace ScrubJay.Enums;

partial class EnumExtensions
{
    extension<E>(E)
        where E : struct, Enum
    {
#region Comparison Operators
        public static bool operator ==(E left, E right)
        {
            Emit.Ldarg(nameof(left));
            Emit.Ldarg(nameof(right));
            Emit.Ceq();
            return Return<bool>();
        }

        public static bool operator !=(E left, E right)
        {
            Emit.Ldarg(nameof(left));
            Emit.Ldarg(nameof(right));
            Emit.Ceq();
            Emit.Ldc_I4_0();
            Emit.Ceq();
            return Return<bool>();
        }

        public static bool operator <(E left, E right)
        {
            Emit.Ldarg(nameof(left));
            Emit.Ldarg(nameof(right));
            Emit.Clt();
            return Return<bool>();
        }

        public static bool operator <=(E left, E right)
        {
            Emit.Ldarg(nameof(left));
            Emit.Ldarg(nameof(right));
            Emit.Cgt();
            Emit.Ldc_I4_0();
            Emit.Ceq();
            return Return<bool>();
        }

        public static bool operator >(E left, E right)
        {
            Emit.Ldarg(nameof(left));
            Emit.Ldarg(nameof(right));
            Emit.Cgt();
            return Return<bool>();
        }

        public static bool operator >=(E left, E right)
        {
            Emit.Ldarg(nameof(left));
            Emit.Ldarg(nameof(right));
            Emit.Clt();
            Emit.Ldc_I4_0();
            Emit.Ceq();
            return Return<bool>();
        }
#endregion

        public static Type UnderlyingType
        {
            get
            {
                return typeof(E).GetEnumUnderlyingType();
            }
        }

        public static bool IsSigned
        {
            get
            {
                var code = Type.GetTypeCode(typeof(E).GetEnumUnderlyingType());
                return code is TypeCode.SByte or TypeCode.Int16 or TypeCode.Int32 or TypeCode.Int64;
            }
        }

        public static bool IsUnsigned
        {
            get
            {
                var code = Type.GetTypeCode(typeof(E).GetEnumUnderlyingType());
                return code is TypeCode.Byte or TypeCode.UInt16 or TypeCode.UInt32 or TypeCode.UInt64;
            }
        }


        internal static unsafe E UnsafeFrom<T>(T value)
            where T : unmanaged
        {
            Emit.Ldarg(nameof(value));
            return Return<E>();
        }

        public static Result<E> TryFrom(long i64)
        {
            var typeCode = Type.GetTypeCode(get_UnderlyingType<E>());
            // order of likelyhood
            if (typeCode == TypeCode.Int32)
            {
                if (i64 < int.MinValue || i64 > int.MaxValue)
                    return Ex.Parse<long, E>(i64, $"out of range for underlying type {Type.Render<int>()}");
                return Result<E>.Ok(Notsafe.As<int, E>((int)i64));
            }
            else if (typeCode == TypeCode.Byte)
            {
                if (i64 < byte.MinValue || i64 > byte.MaxValue)
                    return Ex.Parse<long, E>(i64, $"out of range for underlying type {Type.Render<byte>()}");
                return Result<E>.Ok(Notsafe.As<byte, E>((byte)i64));
            }
            else if (typeCode == TypeCode.Int64)
            {
                return Result<E>.Ok(Notsafe.As<long, E>(i64));
            }
            else if (typeCode == TypeCode.Int16)
            {
                if (i64 < short.MinValue || i64 > short.MaxValue)
                    return Ex.Parse<long, E>(i64, $"out of range for underlying type {Type.Render<short>()}");
                return Result<E>.Ok(Notsafe.As<short, E>((short)i64));
            }
            else if (typeCode == TypeCode.UInt32)
            {
                if (i64 < uint.MinValue || i64 > uint.MaxValue)
                    return Ex.Parse<long, E>(i64, $"out of range for underlying type {Type.Render<uint>()}");
                return Result<E>.Ok(Notsafe.As<uint, E>((uint)i64));
            }
            else if (typeCode == TypeCode.UInt16)
            {
                if (i64 < ushort.MinValue || i64 > ushort.MaxValue)
                    return Ex.Parse<long, E>(i64, $"out of range for underlying type {Type.Render<ushort>()}");
                return Result<E>.Ok(Notsafe.As<ushort, E>((ushort)i64));
            }
            else if (typeCode == TypeCode.SByte)
            {
                if (i64 < sbyte.MinValue || i64 > sbyte.MaxValue)
                    return Ex.Parse<long, E>(i64, $"out of range for underlying type {Type.Render<sbyte>()}");
                return Result<E>.Ok(Notsafe.As<sbyte, E>((sbyte)i64));
            }
            else if (typeCode == TypeCode.UInt64)
            {
                if (i64 < 0L)
                    return Ex.Parse<long, E>(i64, $"out of range for underlying type {Type.Render<ulong>()}");
                return Result<E>.Ok(Notsafe.As<ulong, E>((ulong)i64));
            }
            else
            {
                throw Ex.ArgRange(typeCode, "Is not a valid Enum Underlying Type");
            }
        }

        public static Result<E> TryFrom(ulong u64)
        {
            var typeCode = Type.GetTypeCode(get_UnderlyingType<E>());
            // order of likelyhood
            if (typeCode == TypeCode.Int32)
            {
                if (u64 > int.MaxValue)
                    return Ex.Parse<ulong, E>(u64, $"out of range for underlying type {Type.Render<int>()}");
                return Result<E>.Ok(Notsafe.As<int, E>((int)u64));
            }
            else if (typeCode == TypeCode.Byte)
            {
                if (u64 > byte.MaxValue)
                    return Ex.Parse<ulong, E>(u64, $"out of range for underlying type {Type.Render<byte>()}");
                return Result<E>.Ok(Notsafe.As<byte, E>((byte)u64));
            }
            else if (typeCode == TypeCode.Int64)
            {
                if (u64 > long.MaxValue)
                    return Ex.Parse<ulong, E>(u64, $"out of range for underlying type {Type.Render<long>()}");
                return Result<E>.Ok(Notsafe.As<long, E>((long)u64));
            }
            else if (typeCode == TypeCode.Int16)
            {
                if (u64 > (ulong)short.MaxValue)
                    return Ex.Parse<ulong, E>(u64, $"out of range for underlying type {Type.Render<short>()}");
                return Result<E>.Ok(Notsafe.As<short, E>((short)u64));
            }
            else if (typeCode == TypeCode.UInt32)
            {
                if (u64 > uint.MaxValue)
                    return Ex.Parse<ulong, E>(u64, $"out of range for underlying type {Type.Render<uint>()}");
                return Result<E>.Ok(Notsafe.As<uint, E>((uint)u64));
            }
            else if (typeCode == TypeCode.UInt16)
            {
                if (u64 > ushort.MaxValue)
                    return Ex.Parse<ulong, E>(u64, $"out of range for underlying type {Type.Render<ushort>()}");
                return Result<E>.Ok(Notsafe.As<ushort, E>((ushort)u64));
            }
            else if (typeCode == TypeCode.SByte)
            {
                if (u64 > (ulong)sbyte.MaxValue)
                    return Ex.Parse<ulong, E>(u64, $"out of range for underlying type {Type.Render<sbyte>()}");
                return Result<E>.Ok(Notsafe.As<sbyte, E>((sbyte)u64));
            }
            else if (typeCode == TypeCode.UInt64)
            {
                return Result<E>.Ok(Notsafe.As<ulong, E>(u64));
            }
            else
            {
                throw Ex.ArgRange(typeCode, "Is not a valid Enum Underlying Type");
            }
        }

#if NET7_0_OR_GREATER
        public static Result<E> TryFrom<T>(T value)
            where T : unmanaged, IBinaryInteger<T>
        {
            try
            {
                if (T.IsPositive(value))
                {
                    ulong u64 = ulong.CreateChecked(value);
                    return TryFrom<E, ulong>(u64);
                }
                else
                {
                    long i64 = long.CreateChecked(value);
                    return TryFrom<E, long>(i64);
                }
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
#else


#endif
    }

    extension<E>(E @enum)
        where E : struct, Enum
    {
        public int ToInt32()
        {
            Emit.Ldarg(nameof(@enum));
            Emit.Conv_I4();
            return Return<int>();
        }

        public uint ToUInt32()
        {
            Emit.Ldarg(nameof(@enum));
            Emit.Conv_U4();
            return Return<uint>();
        }

        public long ToInt64()
        {
            Emit.Ldarg(nameof(@enum));
            Emit.Conv_I8();
            return Return<long>();
        }

        public ulong ToUInt64()
        {
            Emit.Ldarg(nameof(@enum));
            Emit.Conv_U8();
            return Return<ulong>();
        }



        /// <summary>
        /// Is this <typeparamref name="E"/> <see langword="enum"/> the default?
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if <c>enum == default(E)</c>; otherwise <see langword="false"/>.
        /// </returns>
        public bool IsDefault()
        {
            Emit.Ldarg(nameof(@enum));
            Emit.Ldc_I4_0();
            Emit.Ceq();
            return Return<bool>();
        }

        public bool Equate(E other)
        {
            Emit.Ldarg(nameof(@enum));
            Emit.Ldarg(nameof(other));
            Emit.Ceq();
            return Return<bool>();
        }

        public int Compare(E other)
        {
            if (EnumInfo<E>.IsSigned)
            {
                // if @enum < other return -1
                Emit.Ldarg(nameof(@enum));
                Emit.Ldarg(nameof(other));
                Emit.Clt();
                Emit.Brtrue("lessThan");

                // if @enum > other return 1
                Emit.Ldarg(nameof(@enum));
                Emit.Ldarg(nameof(other));
                Emit.Cgt();
                Emit.Brtrue("greaterThan");

                // else return 0
                Emit.Ldc_I4_0();
                Emit.Ret();
            }
            else
            {
                // if @enum < other return -1
                Emit.Ldarg(nameof(@enum));
                Emit.Ldarg(nameof(other));
                Emit.Clt_Un();
                Emit.Brtrue("lessThan");

                // if @enum > other return 1
                Emit.Ldarg(nameof(@enum));
                Emit.Ldarg(nameof(other));
                Emit.Cgt_Un();
                Emit.Brtrue("greaterThan");

                // else return 0
                Emit.Ldc_I4_0();
                Emit.Ret();
            }

            MarkLabel("lessThan");
            Emit.Ldc_I4_M1();
            Emit.Ret();

            MarkLabel("greaterThan");
            Emit.Ldc_I4_1();
            Emit.Ret();
            throw Unreachable();
        }

        public bool IsEqualTo(E other)
        {
            Emit.Ldarg(nameof(@enum));
            Emit.Ldarg(nameof(other));
            Emit.Ceq();
            return Return<bool>();
        }

        public bool IsNotEqualTo(E other)
        {
            Emit.Ldarg(nameof(@enum));
            Emit.Ldarg(nameof(other));
            Emit.Ceq();
            Emit.Ldc_I4_0();
            Emit.Ceq();
            return Return<bool>();
        }

        public bool IsLessThan(E other)
        {
            if (EnumInfo<E>.IsSigned)
            {
                Emit.Ldarg(nameof(@enum));
                Emit.Ldarg(nameof(other));
                Emit.Clt();
                return Return<bool>();
            }
            else
            {
                Emit.Ldarg(nameof(@enum));
                Emit.Ldarg(nameof(other));
                Emit.Clt_Un();
                return Return<bool>();
            }
        }

        public bool IsLessThanOrEqualTo(E other)
        {
            if (EnumInfo<E>.IsSigned)
            {
                Emit.Ldarg(nameof(@enum));
                Emit.Ldarg(nameof(other));
                Emit.Cgt();
                Emit.Ldc_I4_0();
                Emit.Ceq();
                return Return<bool>();
            }
            else
            {
                Emit.Ldarg(nameof(@enum));
                Emit.Ldarg(nameof(other));
                Emit.Cgt_Un();
                Emit.Ldc_I4_0();
                Emit.Ceq();
                return Return<bool>();
            }
        }

        public bool IsGreaterThan(E other)
        {
            if (EnumInfo<E>.IsSigned)
            {
                Emit.Ldarg(nameof(@enum));
                Emit.Ldarg(nameof(other));
                Emit.Cgt();
                return Return<bool>();
            }
            else
            {
                Emit.Ldarg(nameof(@enum));
                Emit.Ldarg(nameof(other));
                Emit.Cgt_Un();
                return Return<bool>();
            }
        }

        public bool IsGreaterThanOrEqualTo(E other)
        {
            if (EnumInfo<E>.IsSigned)
            {
                Emit.Ldarg(nameof(@enum));
                Emit.Ldarg(nameof(other));
                Emit.Clt();
                Emit.Ldc_I4_0();
                Emit.Ceq();
                return Return<bool>();
            }
            else
            {
                Emit.Ldarg(nameof(@enum));
                Emit.Ldarg(nameof(other));
                Emit.Clt_Un();
                Emit.Ldc_I4_0();
                Emit.Ceq();
                return Return<bool>();
            }
        }
    }

}