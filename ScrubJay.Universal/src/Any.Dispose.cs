// ReSharper disable MethodOverloadWithOptionalParameter

namespace ScrubJay.Universal;

public partial class Any
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Dispose<T>([AllowNull, MaybeNull] ref T? instance)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        if (instance is not null)
        {
            DisposeCache<T>.Dispose(ref instance);
        }
        
        // set to default to remove all references
        instance = default(T);
    }

    private static class DisposeCache<T>
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        internal static readonly AnyDispose<T> Dispose;

        static DisposeCache()
        {
            Type instanceType = typeof(T);
            MethodInfo? method = instanceType
                .FindMatchingMethods<Action<T>>(nameof(IDisposable.Dispose))
                .FirstOrDefault();

            if (method is not null && DynamicMethod.TryGenerateDelegate<AnyDispose<T>>(
                $"Any_{instanceType}_Dispose", 
                gen => gen
                    .Ldarg(0)
                    .Constrained(instanceType)
                    .Callvirt(method)
                    .Ret(),
                out Dispose!))
            {
                return;
            }

            Dispose = FallbackDispose;
        }

        private static void FallbackDispose(ref T? instance)
        {
            // do nothing
        }
    }
}