// ReSharper disable StaticMemberInGenericType

// ReSharper disable MethodOverloadWithOptionalParameter
namespace ScrubJay.Polyfills.Universal;

partial class Any
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Type GetType<T>(in T? value)
    {
        if (value is null)
            return typeof(T);
        return value.GetType();
    }

#if NET9_0_OR_GREATER

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Type GetType<T>(in T? value, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        if (value is null)
            return typeof(T);
        return GetTypeCache<T>.Invoke(in value);
    }

    internal delegate Type AnyGetType<T>(in T value)
        where T : allows ref struct;

    internal static class GetTypeCache<T>
        where T : allows ref struct
    {
        public static readonly AnyGetType<T> Invoke;

        static GetTypeCache()
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
                    .Where(static m => string.Equals(m.Name, nameof(object.GetType), StringComparison.Ordinal))
                    .Where(static m => m.ReturnType == typeof(Type))
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

            if (method is not null && DynamicMethod.TryGenerateDelegate<AnyGetType<T>>($"Any_{type}_GetType",
                gen =>
                {
                    gen.Emit(OpCodes.Ldarg_0);
                    gen.Emit(OpCodes.Constrained, type);
                    gen.Emit(OpCodes.Callvirt, method);
                    gen.Emit(OpCodes.Ret);
                }, out Invoke!))
            {
                return;
            }

            Invoke = GetTypeFallback;
        }

        private static Type GetTypeFallback(in T? _) => typeof(T);
    }
#endif
}