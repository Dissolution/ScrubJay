#pragma warning disable IDE0060, CA1045
// ReSharper disable EntityNameCapturedOnly.Global

#if NET6_0_OR_GREATER
using InlineIL;
#endif

using static InlineIL.IL;


namespace ScrubJay.Enums;

/// <summary>
/// Generic (non-boxing) extensions upon <c>[Flags] enum</c>
/// </summary>
/// <remarks>
/// There is no way to constrict these extensions to flagged enums,
/// so beware of odd behavior for non-flagged enums
/// </remarks>
[PublicAPI]
public static class FlagsEnumExtensions
{
    extension<E>(E)
        where E : struct, Enum
    {
        public static E operator &(E left, E right) => left.And(right);
        public static E operator |(E left, E right) => left.Or(right);
        public static E operator ^(E left, E right) => left.Xor(right);
        public static E operator !(E @enum) => @enum.BitwiseComplement();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static E From(long i64)
        {
            Emit.Ldarg(nameof(i64));
            return Return<E>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static E From(ulong u64)
        {
            Emit.Ldarg(nameof(u64));
            return Return<E>();
        }
    }

    extension<E>(ref E @enum)
        where E : struct, Enum
    {
        /// <summary>
        /// Adds the <paramref name="flag"/> to this <paramref name="enum"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddFlag(E flag) => @enum = @enum.Or(flag);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveFlag(E flag) => @enum = @enum.And(flag.BitwiseComplement());
    }


    extension<E>(E @enum)
        where E : struct, Enum
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public sbyte ToI8()
        {
            Emit.Ldarg(nameof(@enum));
            Emit.Conv_I1();
            return Return<sbyte>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte ToU8()
        {
            Emit.Ldarg(nameof(@enum));
            Emit.Conv_U1();
            return Return<byte>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public short ToI16()
        {
            Emit.Ldarg(nameof(@enum));
            Emit.Conv_I2();
            return Return<short>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ushort ToU16()
        {
            Emit.Ldarg(nameof(@enum));
            Emit.Conv_U2();
            return Return<ushort>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ToI32()
        {
            Emit.Ldarg(nameof(@enum));
            Emit.Conv_I4();
            return Return<int>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint ToU32()
        {
            Emit.Ldarg(nameof(@enum));
            Emit.Conv_U4();
            return Return<uint>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long ToI64()
        {
            Emit.Ldarg(nameof(@enum));
            Emit.Conv_I8();
            return Return<long>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong ToU64()
        {
            Emit.Ldarg(nameof(@enum));
            Emit.Conv_U8();
            return Return<ulong>();
        }

        /// <summary>
        /// Returns a bitwise NOT (<c>~</c>) of this <paramref name="enum"/>
        /// </summary>
        /// <remarks>
        /// Also known as the inverse or bitwise complement
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public E BitwiseComplement()
        {
            Emit.Ldarg(nameof(@enum));
            Emit.Not();
            return Return<E>();
        }

        /// <summary>
        /// Returns a bitwise AND (<c>&amp;</c>) of the <paramref name="enum"/> and the <paramref name="flag"/>
        /// </summary>
        /// <param name="flag">The second <typeparamref name="E"/> to AND</param>
        /// <returns>The two <see langword="enum"/>s AND'd together</returns>
        /// <remarks>
        /// <c>return (<paramref name="enum"/> &amp; <paramref name="flag"/>);</c>
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public E And(E flag)
        {
            Emit.Ldarg(nameof(@enum));
            Emit.Ldarg(nameof(flag));
            Emit.And();
            return Return<E>();
        }

        /// <summary>
        /// Returns a bitwise OR (<c>|</c>) of the <paramref name="enum"/> and the <paramref name="flag"/>
        /// </summary>
        /// <param name="flag">The second <typeparamref name="E"/> to OR</param>
        /// <returns>The two <see langword="enum"/>s OR'd together</returns>
        /// <remarks>
        /// <c>return (<paramref name="enum"/> | <paramref name="flag"/>);</c>
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public E Or(E flag)
        {
            Emit.Ldarg(nameof(@enum));
            Emit.Ldarg(nameof(flag));
            Emit.Or();
            return Return<E>();
        }

        /// <summary>
        /// Returns a bitwise XOR (<c>^</c>) of the <paramref name="enum"/> and the <paramref name="flag"/>
        /// </summary>
        /// <param name="flag">The second <typeparamref name="E"/> to XOR</param>
        /// <returns>The two <see langword="enum"/>s XOR'd together</returns>
        /// <remarks>
        /// <c>return (<paramref name="enum"/> ^ <paramref name="flag"/>);</c>
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public E Xor(E flag)
        {
            Emit.Ldarg(nameof(@enum));
            Emit.Ldarg(nameof(flag));
            Emit.Xor();
            return Return<E>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasOneFlag()
        {
            // is power of 2
            // return (x & (x - 1)) == 0

            Emit.Ldarg(nameof(@enum));
            Emit.Ldarg(nameof(@enum));
            Emit.Ldc_I4_1();
            Emit.Sub();
            Emit.And();
            Emit.Ldc_I4_0();
            Emit.Ceq();
            return Return<bool>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasMultipleFlags() => !@enum.HasOneFlag() && !@enum.IsDefault;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int FlagCount()
        {
#if NET6_0_OR_GREATER
            Emit.Ldarg(nameof(@enum));
            Emit.Conv_U8();
            Emit.Call(MethodRef.Method(typeof(BitOperations), nameof(BitOperations.PopCount), typeof(ulong)));
            return Return<int>();
#else
        const ulong MASK1 = 0b1010101010101010101010101010101_01010101010101010101010101010101UL;
        const ulong MASK2 = 0b0110011001100110011001100110011_00110011001100110011001100110011UL;
        const ulong MASK3 = 0b0001111000011110000111100001111_00001111000011110000111100001111UL;
        const ulong MASK4 = 0b0000001000000010000000100000001_00000001000000010000000100000001UL;

        ulong value = ToUInt64<E>(@enum);
        value -= (value >> 1) & MASK1;
        value = (value & MASK2) + ((value >> 2) & MASK2);
        value = (((value + (value >> 4)) & MASK3) * MASK4) >> 56;
        return (int)value;
#endif
        }

        public E[] GetFlags()
        {
            int flagCount = @enum.FlagCount();
            var flags = new E[flagCount];
            int f = 0;
            int maxBits = Unsafe.SizeOf<E>() * 8;
            ulong enumValue = @enum.ToU64();
            for (int shift = 0; shift < maxBits; shift++)
            {
                ulong mask = 1UL << shift;
                if ((enumValue & mask) != 0UL)
                {
                    var flag = From<E>(mask);
                    flags[f++] = flag;
                }
            }
            Debug.Assert(f == flagCount);
            return flags;
        }

        public IEnumerable<E> EnumerateFlags()
        {
            int maxBits = Unsafe.SizeOf<E>() * 8;
            ulong enumValue = @enum.ToU64();
            for (int shift = 0; shift < maxBits; shift++)
            {
                ulong mask = 1UL << shift;
                if ((enumValue & mask) != 0UL)
                {
                    yield return From<E>(mask);
                }
            }
        }

        /// <summary>
        /// Returns this <paramref name="enum"/> combined with the <paramref name="flag"/>
        /// </summary>
        /// <remarks>
        /// <c>return</c> <paramref name="enum"/> <c>|</c> <paramref name="flag"/>
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public E WithFlag(E flag) => @enum.Or(flag);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public E WithoutFlag(E flag) => @enum.And(flag.BitwiseComplement());

        [OverloadResolutionPriority(100)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasFlag(E flag)
        {
            Emit.Ldarg(nameof(flag));
            Emit.Dup();
            Emit.Ldarg(nameof(@enum));
            Emit.And();
            Emit.Ceq();
            return Return<bool>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasFlags(E flag) => @enum.HasFlag<E>(flag);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasAnyFlags(E flag) => @enum.HasFlag<E>(flag);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasAnyFlags(E flag1, E flag2)
        {
            return (@enum & (flag1 | flag2)) != default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasAnyFlags(E flag1, E flag2, E flag3)
        {
            return (@enum & (flag1 | flag2 | flag3)) != default;
        }

        public bool HasAnyFlags(params ReadOnlySpan<E> flags)
        {
            E flag = default;
            for (int i = 0; i < flags.Length; i++)
            {
                flag.AddFlag(flags[i]);
            }

            return @enum.HasFlag<E>(flag);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasAllFlags(E flag)
        {
            return (@enum & flag) == flag;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasAllFlags(E flag1, E flag2)
        {
            E flags = flag1 | flag2;
            return (@enum & flags) == flags;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasAllFlags(E flag1, E flag2, E flag3)
        {
            E flags = flag1 | flag2 | flag3;
            return (@enum & flags) == flags;
        }

        public bool HasAllFlags(params ReadOnlySpan<E> flags)
        {
            E flag = default;
            for (int i = 0; i < flags.Length; i++)
            {
                flag.AddFlag(flags[i]);
            }

            return (@enum & flag) == flag;
        }
    }
}