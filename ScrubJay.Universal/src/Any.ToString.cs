using System.Reflection;
using System.Reflection.Emit;
// ReSharper disable MethodOverloadWithOptionalParameter

namespace ScrubJay.Universal;

public partial class Any
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNullIfNotNull(nameof(value))]
    public static string? ToString<T>(in T? value)
    {
        return value?.ToString();
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNullIfNotNull(nameof(value))]
    public static string? ToString<T>(in T? value, TypeConstraints.AllowsRefStruct<T> _ = default)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        if (value is null)
            return null;
        return ToStringCache<T>.Invoke(in value);
    }


    private static class ToStringCache<T>
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        internal static readonly AnyToString<T> Invoke;

        static ToStringCache()
        {
            Type instanceType = typeof(T);
            MethodInfo? method = instanceType
                .FindMatchingMethods<Func<T,string>>(nameof(object.ToString))
                .FirstOrDefault();

            if (method is not null && DynamicMethod.TryGenerateDelegate<AnyToString<T>>(
                $"Any_{instanceType}_ToString",
                    gen => gen
                        .Ldarg(0)
                        .Constrained(instanceType)
                        .Callvirt(method)
                        .Ret(), out Invoke!))
            {
                return;
            }

            Invoke = FallbackToString;
        }
        
        private static string FallbackToString(in T? instance) => typeof(T).ToString(); // same as object.ToString()
    }
}