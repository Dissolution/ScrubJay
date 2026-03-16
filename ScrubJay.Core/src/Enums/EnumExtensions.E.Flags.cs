//using InlineIL;
//using static InlineIL.IL;
//
//namespace ScrubJay.Enums;
//
//static partial class EnumExtensions
//{
//
//    extension<E>(E @enum)
//        where E : struct, Enum
//    {

//        public int FlagCount
//        {
//            get
//            {
//#if NET6_0_OR_GREATER
//                Emit.Ldarg(nameof(@enum));
//                Emit.Conv_U8();
//                Emit.Call(MethodRef.Method(typeof(BitOperations), nameof(BitOperations.PopCount), typeof(ulong)));
//                return Return<int>();
//#else
//                // fallback from BitOperations.PopCount
//
//                const ulong c1 = 0x_55555555_55555555ul;
//                const ulong c2 = 0x_33333333_33333333ul;
//                const ulong c3 = 0x_0F0F0F0F_0F0F0F0Ful;
//                const ulong c4 = 0x_01010101_01010101ul;
//
//                ulong value = @enum.ToUInt64();
//                value -= (value >> 1) & c1;
//                value = (value & c2) + ((value >> 2) & c2);
//                value = (((value + (value >> 4)) & c3) * c4) >> 56;
//
//                return (int)value;
//#endif
//            }
//        }
//
//        public IEnumerable<E> EnumerateFlags()
//        {
//            ulong enumValue;
//            
//            if (get_IsSigned<E>())
//            {
//                long value = @enum.ToInt64();
//                if (value < 0L)
//                    yield break;
//                enumValue = (ulong)value;
//            }
//            else
//            {
//                enumValue = @enum.ToUInt64();
//            }
//
//            ulong mask = 1UL;
//            for (var i = 0; i < 64; i++,mask<<=1)
//            {
//                if ((enumValue & mask) == mask)
//                {
//                    yield return Notsafe.As<ulong, E>(mask);
//                }
//            }
//        }
//
//        public E[] GetFlags()
//        {
//            int flagCount = @enum.FlagCount;
//            var flags = new E[flagCount];
//            int f = 0;
//            int maxBits = Unsafe.SizeOf<E>() * 8;
//            ulong enumValue = @enum.ToUInt64();
//            for (int shift = 0; shift < maxBits; shift++)
//            {
//                ulong mask = 1UL << shift;
//                if ((enumValue & mask) != 0UL)
//                {
//                    var flag = Notsafe.As<ulong, E>(mask);
//                    flags[f++] = flag;
//                }
//            }
//            Debug.Assert(f == flagCount);
//            return flags;
//        }
//        
//        
//#endregion
