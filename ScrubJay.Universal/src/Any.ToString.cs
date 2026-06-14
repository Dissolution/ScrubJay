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

#if NET9_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNullIfNotNull(nameof(value))]
    public static string? ToString<T>(in T? value, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        if (value is null)
            return null;
        return ToStringCache<T>.Invoke(in value);
    }
#endif


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
                .FindMatchingInstanceMethods("ToString", typeof(string), Type.EmptyTypes)
                .FirstOrDefault();

            if (method is not null && TryGenerateDelegate<AnyToString<T>>(
                $"Any_{instanceType}_ToString",
                gen =>
                {
                    gen.Emit(OpCodes.Ldarg_0);
                    gen.Emit(OpCodes.Constrained, instanceType);
                    gen.Emit(OpCodes.Callvirt, method);
                    gen.Emit(OpCodes.Ret);
                }, out Invoke!))
            {
                return;
            }

            Invoke = Fallback;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string Fallback(in T instance) => typeof(T).ToString(); // same as object.ToString()
    }
}