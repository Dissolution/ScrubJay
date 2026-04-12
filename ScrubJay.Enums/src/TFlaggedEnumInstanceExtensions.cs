//namespace ScrubJay.Enums;
//
///// <summary>
///// Extensions on generic instances constrained to <see langword="struct"/> and <see langword="enum"/>.
///// </summary>
//[PublicAPI]
//public static class TFlaggedEnumInstanceExtensions
//{
//    extension<TEnum>(TEnum @enum)
//        where TEnum : struct, Enum
//    {
//        /// <summary>
//        /// Is this <typeparamref name="TEnum"/> <see langword="enum"/> a non-zero power-of-two?<br/>
//        /// <i>thus likely a single flag value</i>
//        /// </summary>
//        public bool IsPowerOfTwo
//        {
//            [MethodImpl(MethodImplOptions.AggressiveInlining)]
//            get
//            {
//                // (x & (x-1)) == 0
//                Emit.Ldarg(nameof(@enum));
//                Emit.Ldc_I4_1();
//                Emit.Sub();
//                Emit.Ldarg(nameof(@enum));
//                Emit.And();
//                Emit.Ldc_I4_0();
//                Emit.Ceq();
//                return Return<bool>();
//            }
//        }
//
//        /// <summary>
//        /// Get the number of set bit flags in this <typeparamref name="TEnum"/> <see langword="enum"/>.
//        /// </summary>
//        public int FlagCount
//        {
//#if NET6_0_OR_GREATER
//            [MethodImpl(MethodImplOptions.AggressiveInlining)]
//            get => BitOperations.PopCount(@enum.AsU64());
//#else
//            get
//            {
//                ulong value = @enum.AsU64();
//                const ulong c1 = 0x_55555555_55555555ul;
//                const ulong c2 = 0x_33333333_33333333ul;
//                const ulong c3 = 0x_0F0F0F0F_0F0F0F0Ful;
//                const ulong c4 = 0x_01010101_01010101ul;
//
//                value -= (value >> 1) & c1;
//                value = (value & c2) + ((value >> 2) & c2);
//                value = (((value + (value >> 4)) & c3) * c4) >> 56;
//
//                return (int)value;
//                
//}
//#endif
//        }
//
//#region HasFlags
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public bool HasFlag(TEnum flag)
//        {
//            return (@enum & flag) == flag;
//        }
//
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public bool HasFlags(TEnum flag)
//        {
//            return (@enum & flag) == flag;
//        }
//
//#region HasAllFlags
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public bool HasAllFlags(TEnum flag1)
//        {
//            return (@enum & flag1) == flag1;
//        }
//
//        public bool HasAllFlags(TEnum flag1, TEnum flag2)
//        {
//            TEnum combined = flag1 | flag2;
//            return (@enum & combined) == combined;
//        }
//
//        public bool HasAllFlags(TEnum flag1, TEnum flag2, TEnum flag3)
//        {
//            TEnum combined = flag1 | flag2 | flag3;
//            return (@enum & combined) == combined;
//        }
//
//        public bool HasAllFlags(TEnum flag1, TEnum flag2, TEnum flag3, TEnum flag4)
//        {
//            TEnum combined = flag1 | flag2 | flag3 | flag4;
//            return (@enum & combined) == combined;
//        }
//
//        public bool HasAllFlags(TEnum flag1, TEnum flag2, TEnum flag3, TEnum flag4, TEnum flag5)
//        {
//            TEnum combined = flag1 | flag2 | flag3 | flag4 | flag5;
//            return (@enum & combined) == combined;
//        }
//
//        public bool HasAllFlags(params ReadOnlySpan<TEnum> flags)
//        {
//            TEnum combined = default;
//            foreach (var flag in flags)
//            {
//                combined |= flag;
//            }
//            return (@enum & combined) == combined;
//        }
//#endregion /HasAllFlags
//
//#region HasAnyFlags
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public bool HasAnyFlags(TEnum flag1)
//        {
//            return (@enum & flag1) != default;
//        }
//
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public bool HasAnyFlags(TEnum flag1, TEnum flag2)
//        {
//            return (@enum & (flag1 | flag2)) != default;
//        }
//
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public bool HasAnyFlags(TEnum flag1, TEnum flag2, TEnum flag3)
//        {
//            return (@enum & (flag1 | flag2 | flag3)) != default;
//        }
//
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public bool HasAnyFlags(TEnum flag1, TEnum flag2, TEnum flag3, TEnum flag4)
//        {
//            return (@enum & (flag1 | flag2 | flag3 | flag4)) != default;
//        }
//
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public bool HasAnyFlags(TEnum flag1, TEnum flag2, TEnum flag3, TEnum flag4, TEnum flag5)
//        {
//            return (@enum & (flag1 | flag2 | flag3 | flag4 | flag5)) != default;
//        }
//
//        public bool HasAnyFlags(params ReadOnlySpan<TEnum> flags)
//        {
//            TEnum combined = default;
//            foreach (var flag in flags)
//            {
//                combined |= flag;
//            }
//            return (@enum & combined) != default;
//        }
//#endregion /HasAnyFlags
//#endregion /HasFlags
//
//        public TEnum[] GetFlags()
//        {
//            int flagCount = @enum.FlagCount;
//            var flags = new TEnum[flagCount];
//            int f = 0;
//            int maxBits = Unsafe.SizeOf<TEnum>() * 8;
//            ulong enumValue = @enum.AsU64();
//            for (int shift = 0; shift < maxBits; shift++)
//            {
//                ulong mask = 1UL << shift;
//                if ((enumValue & mask) == mask)
//                {
//                    var flag = TEnumExtensions.From<TEnum>(mask);
//                    flags[f++] = flag;
//                }
//            }
//            Debug.Assert(f == flagCount);
//            return flags;
//        }
//
//#region With
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public TEnum WithFlag(TEnum flag) => @enum | flag;
//
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public TEnum WithoutFlag(TEnum flag) => @enum & ~flag;
//#endregion
//    }
//
//    extension<TEnum>(ref TEnum @enum)
//        where TEnum : struct, Enum
//    {
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public void AddFlag(TEnum flag)
//        {
//            @enum |= flag;
//        }
//
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public void RemoveFlag(TEnum flag)
//        {
//            @enum &= ~flag;
//        }
//    }
//}