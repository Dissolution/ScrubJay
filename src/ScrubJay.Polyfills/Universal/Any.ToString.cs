using System.Reflection;
using System.Reflection.Emit;
using ScrubJay.Reflection.Lightweight;
// ReSharper disable StaticMemberInGenericType
#if NET9_0_OR_GREATER

namespace ScrubJay.Polyfills.Universal;

partial class Any
{
    internal delegate string AnyToString<T>(in T value)
        where T : allows ref struct;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasToString<T>()
        where T : allows ref struct
        => ToStringCache<T>.FoundMethod;

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

    public static string? ToStringOrDefault<T>(in T? value)
        where T : allows ref struct
    {
        if (value is not null && ToStringCache<T>.FoundMethod)
        {
            return ToStringCache<T>.Invoke(in value);
        }
        return null;
    }



    internal static class ToStringCache<T>
        where T : allows ref struct
    {
        public static readonly bool FoundMethod;
        public static readonly AnyToString<T> Invoke;

        static ToStringCache()
        {
            var type = typeof(T);

            IEnumerable<Type> searchTypes;
            if (type.IsByRefLike)
            {
                searchTypes = [type];
            }
            else
            {
                searchTypes = type.EnumerateTypeAndBaseTypes();
            }
            
            var methods = searchTypes
                .SelectMany(static t => t
                    .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(static m => string.Equals(m.Name, nameof(object.ToString), StringComparison.Ordinal))
                    .Where(static m => m.ReturnType == typeof(string))
                    .Where(static m => m.GetParameters().Length == 0)
                )
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
            
            if (method is not null && DynamicMethod.TryGenerateDelegate<AnyToString<T>>($"Any_{type}_ToString",
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
}


#endif