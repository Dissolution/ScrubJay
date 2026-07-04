// ReSharper disable MethodOverloadWithOptionalParameter

namespace ScrubJay.Universal;

public partial class Any
{
#if !NET9_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasEqualsObject<T>() => true;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equals<T>(in T? value, object? other)
    {
        if (value is null)
            return other is null;
        return value.Equals(other!);
    }

#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasEqualsObject<T>() => EqualsObjectCache<T>.FoundMethod;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equals<T>(in T? value, object? other)
        where T : allows ref struct
    {
        if (value is null)
            return other is null;
        return EqualsObjectCache<T>.Invoke(in value, other);
    }

    internal delegate bool AnyEqualsObject<T>(in T value, object? other)
        where T : allows ref struct;

    internal static class EqualsObjectCache<T>
        where T : allows ref struct
    {
        public static readonly bool FoundMethod;
        public static readonly AnyEqualsObject<T> Invoke;

        static EqualsObjectCache()
        {
            Type type = typeof(T);

            var methods = type
                .InvokableTypes()
                .SelectMany(t => t
                    .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .WithShape(nameof(Equals), typeof(bool), [typeof(object)]))
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

            if (method is not null && DynamicMethod.TryGenerateDelegate(
                $"Any_{type}_Equals_Object",
                gen =>
                {
                    gen.Emit(OpCodes.Ldarg_0);
                    gen.Emit(OpCodes.Ldarg_1);
                    gen.Emit(OpCodes.Constrained, type);
                    gen.Emit(OpCodes.Callvirt, method);
                    gen.Emit(OpCodes.Ret);
                }
                , out Invoke!))
            {
                FoundMethod = true;
                return;
            }

            FoundMethod = false;
            Invoke = EqualsObjectFallback;
        }

        private static bool EqualsObjectFallback(in T? instance, object? other) => false;
    }
#endif
}