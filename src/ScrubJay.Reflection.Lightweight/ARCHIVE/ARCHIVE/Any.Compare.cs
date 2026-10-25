//// ReSharper disable MethodOverloadWithOptionalParameter
//
//namespace ScrubJay.Universal;
//
//public partial class Any
//{
//    public static int Compare<T>(in T? instance, in T? other)
//        where T : IComparable<T>
//#if NET9_0_OR_GREATER
//        , allows ref struct
//#endif
//    {
//        if (instance is not null)
//        {
//            return instance.CompareTo(other!);
//        }
//
//        if (other is not null)
//            return -1;
//
//        return 0;
//    }
//
//    public static int Compare<T>(in T? instance, in T? other, TypeConstraints.AllowsRefStruct<T> _ = default)
//#if NET9_0_OR_GREATER
//        where T : allows ref struct
//#endif
//    {
//        if (instance is not null)
//        {
//            return CompareCache<T>.Compare(in instance, in other);
//        }
//        else if (other is not null)
//        {
//            return -1;
//        }
//        else
//        {
//            return 0;
//        }
//    }
//    
//    private static class CompareCache<T>
//#if NET9_0_OR_GREATER
//        where T : allows ref struct
//#endif
//    {
//        internal static readonly AnyCompare<T> Compare;
//
//        static CompareCache()
//        {
//            Type instanceType = typeof(T);
//
//            MethodInfo? method = instanceType
//                .FindMatchingMethods<Func<T,T,int>>(nameof(IComparable.CompareTo))
//                .FirstOrDefault();
//
//            if (method is not null && DynamicMethod.TryGenerateDelegate<AnyCompare<T>>(
//                $"Any_{instanceType}_CompareTo",
//                gen => gen
//                    .Ldarg(0)
//                    .Ldarg(1)
//                    .Ldobj(instanceType)
//                    .Constrained(instanceType)
//                    .Callvirt(method)
//                    .Ret(), out Compare!))
//            {
//                return;
//            }
//            
//            Compare = FallbackCompare;
//        }
//
//        private static int FallbackCompare(in T? instance, in T? other)
//        {
//            Emit.Ldarg_0();
//            Emit.Ldarg_1();
//            Emit.Clt();
//            Emit.Brtrue("lt");
//            
//            Emit.Ldarg_0();
//            Emit.Ldarg_1();
//            Emit.Cgt();
//            Emit.Brtrue("gt");
//            
//            Emit.Ldc_I4_0();
//            Emit.Ret();
//            
//            MarkLabel("lt");
//            Emit.Ldc_I4_M1();
//            Emit.Ret();
//            
//            MarkLabel("gt");
//            Emit.Ldc_I4_1();
//            Emit.Ret();
//            
//            throw Unreachable();
//        }
//    }
//}