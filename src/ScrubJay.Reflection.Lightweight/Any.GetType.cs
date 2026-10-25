// ReSharper disable StaticMemberInGenericType

// ReSharper disable MethodOverloadWithOptionalParameter

using ScrubJay.Reflection.Lightweight.Extensions;

namespace ScrubJay.Reflection.Lightweight;

public static partial class Any
{
#if !NET9_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Type GetType<T>(in T? value)
    {
        if (value is null)
            return typeof(T);
        return value.GetType();
    }

#else

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Type GetType<T>(in T? value)
        where T : allows ref struct
    {
        return AnyGetTypeCache<T>.Invoke(in value);
    }

    internal delegate Type AnyGetType<T>(in T? value)
        where T : allows ref struct;

    internal static class Testing<T>
        where T : class
    {
        private static Type TestGetType(in T? instance)
        {
            if (instance is null)
                return typeof(void);
            return instance.GetType();
        }
    }

    internal static class AnyGetTypeCache<T>
        where T : allows ref struct
    {
        private static readonly MethodInfo _objectGetTypeMethod = typeof(object).GetMethod(nameof(object.GetType), BindingFlags.Public | BindingFlags.Instance)!;

        public static readonly AnyGetType<T> Invoke = CreateInvoke();

        private static AnyGetType<T> CreateInvoke()
        {
            var type = typeof(T);

            // irreducible types
            if (type.IsValueType || type.IsByRefLike || type.IsByRef || type.IsPointer)
            {
                return (in _) => type;
            }

            // reducible type, need to call GetType on actual instance
            Debug.Assert(type.IsClass || type.IsInterface);

            var method = DynamicMethod.Create<AnyGetType<T>>($"Any_{type}_GetType");
            var generator = method.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            //generator.Emit(OpCodes.Constrained, type);
            generator.Emit(OpCodes.Callvirt, _objectGetTypeMethod);
            generator.Emit(OpCodes.Ret);

            return method.CreateDelegate();
        }
    }
#endif
}