// ReSharper disable StaticMemberInGenericType


namespace ScrubJay.Universal;

partial class Any
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNullIfNotNull(nameof(obj))]
    public static string? ToString(object? obj) => obj?.ToString();

#if !NET9_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasToString<T>() => true;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNullIfNotNull(nameof(value))]
    public static string? ToString<T>(in T? value) => value?.ToString();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNullIfNotNull(nameof(fallback))]
    public static string? ToStringOr<T>(in T? value, string? fallback) => value?.ToString() ?? fallback;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string? ToStringOrNull<T>(in T? value) => value?.ToString();

#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasToString<T>() => ToStringCache<T>.FoundMethod;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNullIfNotNull(nameof(value))]
    public static string? ToString<T>(in T? value)
        where T : allows ref struct
    {
        if (value is null)
            return null;

        return ToStringCache<T>.Invoke(in value);
    }

    [return: NotNullIfNotNull(nameof(fallback))]
    public static string? ToStringOr<T>(in T? value, string? fallback)
        where T : allows ref struct
    {
        if (value is not null && ToStringCache<T>.FoundMethod)
        {
            return ToStringCache<T>.Invoke(in value);
        }
        return fallback;
    }

    public static string? ToStringOrNull<T>(in T? value)
        where T : allows ref struct
    {
        if (value is not null && ToStringCache<T>.FoundMethod)
        {
            return ToStringCache<T>.Invoke(in value);
        }
        return null;
    }

    internal delegate string AnyToString<T>(in T value)
        where T : allows ref struct;

    internal static class ToStringCache<T>
        where T : allows ref struct
    {
        public static readonly bool FoundMethod;
        public static readonly AnyToString<T> Invoke;

        static ToStringCache()
        {
            var type = typeof(T);

            var methods = type
                .InvokableTypes()
                .SelectMany(t => t
                    .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .WithShape(nameof(ToString), typeof(string), []))
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

            if (method is not null && DynamicMethod.TryGenerateDelegate<AnyToString<T>>(
                $"Any_{type}_ToString",
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
            Invoke = ToStringFallback;
        }

        private static string ToStringFallback(in T? _) => typeof(T).ToString();
    }
#endif
}