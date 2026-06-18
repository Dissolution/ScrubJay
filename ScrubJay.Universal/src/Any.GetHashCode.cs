// ReSharper disable MethodOverloadWithOptionalParameter

namespace ScrubJay.Universal;

public partial class Any
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetHashCode<T>(in T? instance)
    {
        if (instance is null)
            return 0;
        return instance.GetHashCode();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetHashCode<T>(in T? instance, TypeConstraints.AllowsRefStruct<T> _ = default)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        if (instance is null)
            return 0;
        return GetHashCodeCache<T>.GetHash(in instance);
    }

    private static class GetHashCodeCache<T>
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        internal static readonly AnyGetHashCode<T> GetHash;

        static GetHashCodeCache()
        {
            Type instanceType = typeof(T);
            MethodInfo? method = instanceType
                .FindMatchingMethods<Func<T, int>>(nameof(object.GetHashCode))
                .FirstOrDefault();

            if (method is not null && DynamicMethod.TryGenerateDelegate(
                $"Any_{instanceType}_GetHashCode",
                gen => gen
                    .Ldarg(0)
                    .Constrained(instanceType)
                    .Callvirt(method)
                    .Ret(), out GetHash!))
            {
                return;
            }

            GetHash = FallbackGetHashCode;
        }

        private static int FallbackGetHashCode(in T? instance)
            => Hasher.HashReferenceBytes(in instance);
    }
}