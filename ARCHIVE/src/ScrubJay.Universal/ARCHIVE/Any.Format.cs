//// ReSharper disable MethodOverloadWithOptionalParameter
//
//namespace ScrubJay.Universal;
//
//public partial class Any
//{
//    [return: NotNullIfNotNull(nameof(instance))]
//    public static string? Format<T>(in T? instance, string? format = null, IFormatProvider? provider = null)
//        where T : IFormattable
//#if NET9_0_OR_GREATER
//        , allows ref struct
//#endif
//    {
//        if (instance is null)
//            return null;
//        return instance.ToString(format, provider);
//    }
//
//    [return: NotNullIfNotNull(nameof(instance))]
//    public static string? Format<T>(in T? instance, string? format = null, IFormatProvider? provider = null,
//        TypeConstraints.AllowsRefStruct<T> _ = default)
//#if NET9_0_OR_GREATER
//        where T : allows ref struct
//#endif
//    {
//        if (instance is null)
//            return null;
//        return FormatCache<T>.Format(in instance, format, provider);
//    }
//
//    private static class FormatCache<T>
//#if NET9_0_OR_GREATER
//        where T : allows ref struct
//#endif
//    {
//        internal static readonly AnyFormat<T> Format;
//
//        static FormatCache()
//        {
//            Type instanceType = typeof(T);
//            MethodInfo? method = instanceType
//                .FindMatchingMethods<Func<T, string, IFormatProvider, string>>(nameof(IFormattable.ToString))
//                .FirstOrDefault();
//
//            if (method is not null && DynamicMethod.TryGenerateDelegate(
//                $"Any_{instanceType}_Format",
//                gen => gen
//                    .Ldarg(0)
//                    .Ldarg(1)
//                    .Ldarg(2)
//                    .Constrained(instanceType)
//                    .Callvirt(method)
//                    .Ret(), out Format!))
//            {
//                return;
//            }
//
//            Format = FallbackFormat;
//        }
//
//        private static string FallbackFormat(in T? instance, string? format, IFormatProvider? provider)
//        {
//            return ToStringCache<T>.Invoke(in instance);
//        }
//    }
//}