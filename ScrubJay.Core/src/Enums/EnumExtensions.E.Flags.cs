using InlineIL;
using static InlineIL.IL;

namespace ScrubJay.Enums;

static partial class EnumExtensions
{
    extension<E>(E)
        where E : struct, Enum
    {
#region Bitwise Operators
        /// <summary>
        /// Returns the bitwise complement (<see langword="~"/>) of this <typeparamref name="E"/> <see langword="enum"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static E operator ~(E @enum)
        {
            Emit.Ldarg(nameof(@enum));
            Emit.Not();
            return Return<E>();
        }

        /// <summary>
        /// Returns the bitwise AND (<see langword="&"/>) of these <typeparamref name="E"/> <see langword="enum"/>s.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static E operator &(E left, E right)
        {
            Emit.Ldarg(nameof(left));
            Emit.Ldarg(nameof(right));
            Emit.And();
            return Return<E>();
        }

        /// <summary>
        /// Returns the bitwise OR (<see langword="|"/>) of these <typeparamref name="E"/> <see langword="enum"/>s.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static E operator |(E left, E right)
        {
            Emit.Ldarg(nameof(left));
            Emit.Ldarg(nameof(right));
            Emit.Or();
            return Return<E>();
        }

        /// <summary>
        /// Returns the bitwise XOR (<see langword="^"/>) of these <typeparamref name="E"/> <see langword="enum"/>s.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static E operator ^(E left, E right)
        {
            Emit.Ldarg(nameof(left));
            Emit.Ldarg(nameof(right));
            Emit.Xor();
            return Return<E>();
        }
#endregion
    }

    extension<E>(E @enum)
        where E : struct, Enum
    {
#region Count / Enumerate Flags
        public bool HasOneFlag
        {
            get
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
        }

        public int FlagCount
        {
            get
            {
#if NET6_0_OR_GREATER
                Emit.Ldarg(nameof(@enum));
                Emit.Conv_U8();
                Emit.Call(MethodRef.Method(typeof(BitOperations), nameof(BitOperations.PopCount), typeof(ulong)));
                return Return<int>();
#else
                // fallback from BitOperations.PopCount

                const ulong c1 = 0x_55555555_55555555ul;
                const ulong c2 = 0x_33333333_33333333ul;
                const ulong c3 = 0x_0F0F0F0F_0F0F0F0Ful;
                const ulong c4 = 0x_01010101_01010101ul;

                ulong value = @enum.ToUInt64();
                value -= (value >> 1) & c1;
                value = (value & c2) + ((value >> 2) & c2);
                value = (((value + (value >> 4)) & c3) * c4) >> 56;

                return (int)value;
#endif
            }
        }

        public IEnumerable<E> EnumerateFlags()
        {
            ulong enumValue;
            
            if (get_IsSigned<E>())
            {
                long value = @enum.ToInt64();
                if (value < 0L)
                    yield break;
                enumValue = (ulong)value;
            }
            else
            {
                enumValue = @enum.ToUInt64();
            }

            ulong mask = 1UL;
            for (var i = 0; i < 64; i++,mask<<=1)
            {
                if ((enumValue & mask) == mask)
                {
                    yield return Notsafe.As<ulong, E>(mask);
                }
            }
        }

        public E[] GetFlags()
        {
            int flagCount = @enum.FlagCount;
            var flags = new E[flagCount];
            int f = 0;
            int maxBits = Unsafe.SizeOf<E>() * 8;
            ulong enumValue = @enum.ToUInt64();
            for (int shift = 0; shift < maxBits; shift++)
            {
                ulong mask = 1UL << shift;
                if ((enumValue & mask) != 0UL)
                {
                    var flag = Notsafe.As<ulong, E>(mask);
                    flags[f++] = flag;
                }
            }
            Debug.Assert(f == flagCount);
            return flags;
        }
        
        
#endregion

#region Has()Flag()
        public bool HasFlag(E flag) => (@enum & flag) != default;

        public bool HasFlags(E flag) => (@enum & flag) != default;

        public bool HasAnyFlags(E flag) => (@enum & flag) != default;

        public bool HasAnyFlags(E flag1, E flag2) => (@enum & (flag1 | flag2)) != default;

        public bool HasAnyFlags(E flag1, E flag2, E flag3)
            => (@enum & (flag1 | flag2 | flag3)) != default;

        public bool HasAnyFlags(params ReadOnlySpan<E> flags)
        {
            E combined = default;
            foreach (E flag in flags)
            {
                combined |= flag;
            }
            return (@enum & combined) != default;
        }


        public bool HasAllFlags(E flag) => (@enum & flag) == flag;

        public bool HasAllFlags(E flag1, E flag2)
        {
            E combined = flag1 | flag2;
            return (@enum & combined) == combined;
        }


        public bool HasAllFlags(E flag1, E flag2, E flag3)
        {
            E combined = flag1 | flag2 | flag3;
            return (@enum & combined) == combined;
        }

        public bool HasAllFlags(params ReadOnlySpan<E> flags)
        {
            E combined = default;
            foreach (var flag in flags)
            {
                combined |= flag;
            }

            return (@enum & combined) == combined;
        }
#endregion

#region With
        public E WithFlag(E flag) => @enum | flag;

        public E WithoutFlag(E flag) => @enum & ~flag;
#endregion

    }

#region ref E
    extension<E>(ref E @enum)
        where E : struct, Enum
    {
        public void AddFlag(E flag) => @enum |= flag;
        
        public void RemoveFlag(E flag) => @enum &= ~flag;

    }
#endregion
}