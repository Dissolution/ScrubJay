//// ReSharper disable MethodOverloadWithOptionalParameter
//
//namespace ScrubJay.Universal;
//
//public partial class Any
//{
//#if NET7_0_OR_GREATER
//    public static bool TryParse<T>(
//        [AllowNull, NotNullWhen(true)] string? str,
//        [MaybeNullWhen(false)] out T instance)
//        where T : IParsable<T>
//    {
//        return T.TryParse(str, null, out instance);
//    }
//#endif
//
//#if NET7_0_OR_GREATER
//    public static bool TryParse<T>(
//        [AllowNull, NotNullWhen(true)] string? str,
//        IFormatProvider? provider,
//        [MaybeNullWhen(false)] out T instance)
//        where T : IParsable<T>
//    {
//        return T.TryParse(str, provider, out instance);
//    }
//#endif
//    
//    public static bool HasTryParseString<T>()
//#if NET9_0_OR_GREATER
//        where T : allows ref struct
//#endif
//    {
//        return TryParseStringCache<T>.HasInvoke;
//    }
//    
//    public static bool TryParse<T>(
//        [AllowNull, NotNullWhen(true)] string? str,
//        [MaybeNullWhen(false)] out T instance,
//        TypeConstraints.AllowsRefStruct<T> _ = default)
//#if NET9_0_OR_GREATER
//        where T : allows ref struct
//#endif
//    {
//        return TryParseStringCache<T>.Invoke(str, null, out instance);
//    }
//    
//    public static bool TryParse<T>(
//        [AllowNull, NotNullWhen(true)] string? str,
//        IFormatProvider? provider,
//        [MaybeNullWhen(false)] out T instance,
//        TypeConstraints.AllowsRefStruct<T> _ = default)
//#if NET9_0_OR_GREATER
//        where T : allows ref struct
//#endif
//    {
//        return TryParseStringCache<T>.Invoke(str, provider, out instance);
//    }
//
//
//    private static class TryParseStringCache<T>
//#if NET9_0_OR_GREATER
//        where T : allows ref struct
//#endif
//    {
//        internal static readonly bool HasInvoke;
//        internal static readonly AnyTryParseString<T> Invoke;
//
//        static TryParseStringCache()
//        {
//            Type type = typeof(T);
//            MethodInfo? method = type
//                .FindMatchingMethods(
//                    "TryParse",
//                    typeof(bool),
//                    [typeof(string), typeof(IFormatProvider), type.MakeByRefType()])
//                .FirstOrDefault();
//
//            if (method is not null && DynamicMethod.TryGenerateDelegate<AnyTryParseString<T>>(
//                $"Any_{type}_TryParse_String",
//                gen => gen
//                    .Ldarg(0)
//                    .Ldarg(1)
//                    .Ldarg(2)
//                    .Call(method)
//                    .Ret(), out Invoke!))
//            {
//                HasInvoke = true;
//                return;
//            }
//
//            HasInvoke = false;
//            Invoke = FallbackTryParseString;
//        }
//        
//        private static bool FallbackTryParseString(
//            [AllowNull, NotNullWhen(true)] string? str, 
//            IFormatProvider? provider, 
//            [MaybeNullWhen(false)] out T instance)
//        {
//            instance = default;
//            return false;
//        }
//    }
//}