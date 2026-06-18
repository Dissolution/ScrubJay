// ReSharper disable MethodOverloadWithOptionalParameter

namespace ScrubJay.Universal;

public partial class Any
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Type GetType<T>(in T? instance)
    {
        if (instance is null)
            return typeof(T);
        return instance.GetType();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Type GetType<T>(in T? instance, TypeConstraints.AllowsRefStruct<T> _ = default)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        if (instance is null)
            return typeof(T);
        return GetTypeCache<T>.GetTypeOf(in instance);
    }

    private static class GetTypeCache<T>
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        internal static readonly AnyGetType<T> GetTypeOf;

        static GetTypeCache()
        {
            Type instanceType = typeof(T);
            MethodInfo? method = instanceType
                .FindMatchingMethods<Func<T,Type>>(nameof(object.GetType))
                .FirstOrDefault();

            if (method is not null && DynamicMethod.TryGenerateDelegate(
                $"Any_{instanceType}_GetType",
                gen => gen
                    .Ldarg(0)
                    .Constrained(instanceType)
                    .Callvirt(method)
                    .Ret(), out GetTypeOf!))
            {
                return;
            }

            GetTypeOf = FallbackGetType;
        }
        
        private static Type FallbackGetType(in T? instance) => typeof(T);
    }
}