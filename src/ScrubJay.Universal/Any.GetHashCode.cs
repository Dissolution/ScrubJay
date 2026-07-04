namespace ScrubJay.Universal;

public partial class Any
{
#if !NET9_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasGetHashCode<T>() => true;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetHashCode<T>(in T? instance)
    {
        if (instance is null)
            return 0;
        return instance.GetHashCode();
    }
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasGetHashCode<T>() => GetHashCodeCache<T>.FoundMethod;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetHashCode<T>(in T? instance)
        where T : allows ref struct
    {
        if (instance is null)
            return 0;
        return GetHashCodeCache<T>.Invoke(in instance);
    }

    internal delegate int AnyGetHashCode<T>(in T? value)
        where T : allows ref struct;

    internal static class GetHashCodeCache<T>
        where T : allows ref struct
    {
        public static readonly bool FoundMethod;
        public static readonly AnyGetHashCode<T> Invoke;

        static GetHashCodeCache()
        {
            var type = typeof(T);

            var methods = type
                .InvokableTypes()
                .SelectMany(t => t
                    .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .WithShape(nameof(GetHashCode), typeof(int), []))
                .ToList();

            MethodInfo? method;

            if (methods.Count != 1)
            {
                Debugger.Break();
                method = null;
            }
            else
            {
                method = methods[0];
            }

            if (method is not null && DynamicMethod.TryGenerateDelegate<AnyGetHashCode<T>>(
                $"Any_{type}_GetHashCode",
                gen =>
                {
                    gen.Emit(OpCodes.Ldarg_0);
                    gen.Emit(OpCodes.Constrained, type);
                    gen.Emit(OpCodes.Callvirt, method);
                    gen.Emit(OpCodes.Ret);
                }, out Invoke!))
            {
                FoundMethod = true;
                return;
            }

            FoundMethod = false;
            Invoke = GetHashCodeFallback;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe bytesview InToBytes(scoped in T? value)
        {
            if (value is null)
                return [];
            void* ptr = Unsafe.AsPointer<T>(ref Unsafe.AsRef<T>(in value));
            int size = Unsafe.SizeOf<T>();
            return new bytesview(ptr, size);
        }

        private static int GetHashCodeFallback(in T? instance)
        {
            var bytes = InToBytes(in instance);

            // FNV1a - 32 bits
            const uint FNV_OFFSET_BASIS = 2166136261u;
            const uint FNV_PRIME = 16777619u;

            uint hash = FNV_OFFSET_BASIS;

            foreach (byte u8 in bytes)
            {
                hash ^= u8;
                hash *= FNV_PRIME;
            }

            return unchecked((int)hash);
        }
    }
#endif
}