// ReSharper disable MethodOverloadWithOptionalParameter

namespace ScrubJay.Universal;

public partial class Any
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equate<T>(in T? instance, object? other)
    {
        if (instance is null)
            return other is null;
        return instance.Equals(other);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equate<T>(in T? instance, object? other, TypeConstraints.AllowsRefStruct<T> _ = default)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        if (instance is null)
            return other is null;
        return EqualsObjectCache<T>.EquateObject(in instance, other);
    }

    private static class EqualsObjectCache<T>
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        internal static readonly AnyEqualsObject<T> EquateObject;

        static EqualsObjectCache()
        {
            Type instanceType = typeof(T);
            MethodInfo? method = instanceType
                .FindMatchingMethods<Func<T,object,bool>>(nameof(object.Equals))
                .FirstOrDefault();

            if (method is not null && DynamicMethod.TryGenerateDelegate(
                $"Any_{instanceType}_Equals_Object",
                gen => gen
                    .Ldarg(0)
                    .Ldarg(1)
                    .Constrained(instanceType)
                    .Callvirt(method)
                    .Ret(), out EquateObject!))
            {
                return;
            }

            EquateObject = FallbackEqualsObject;
        }

        private static bool FallbackEqualsObject(in T? instance, object? other)
        {
            return false;
        }
    }
}