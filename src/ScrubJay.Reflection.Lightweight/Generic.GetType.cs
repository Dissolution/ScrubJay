// ReSharper disable StaticMemberInGenericType

// ReSharper disable MethodOverloadWithOptionalParameter

using ScrubJay.Reflection.Lightweight.Extensions;

namespace ScrubJay.Reflection.Lightweight;

public static partial class Generic
{
    [OverloadResolutionPriority(100)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Type GetType<T>(T? value)
    {
        if (value is null)
            return typeof(T);
        return value.GetType();
    }

#if NET9_0_OR_GREATER

    [OverloadResolutionPriority(90)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Type GetType<T>(in T? value)
        where T : allows ref struct
    {
        return AnyGetTypeCache<T>.Invoke(in value);
    }

    internal delegate Type AnyGetType<T>(in T? value)
        where T : allows ref struct;

    internal static class AnyGetTypeCache<T>
        where T : allows ref struct
    {
        private static readonly MethodInfo _objectGetTypeMethod = typeof(object).GetMethod(nameof(GetType), BindingFlags.Public | BindingFlags.Instance)!;
        private static readonly MethodInfo _typeGetTypeFromHandleMethod = typeof(Type).GetMethod(nameof(Type.GetTypeFromHandle), BindingFlags.Public | BindingFlags.Static)!;

        public static readonly AnyGetType<T> Invoke = CreateDelegate();

        private static AnyGetType<T> CreateDelegate()
        {
            var type = typeof(T);

            // some types are already basic
            if (type.IsValueType || type.IsByRefLike || type.IsByRef || type.IsPointer)
            {
                return (in _) => type;
            }

            // An instance of T may be more or less specific than T, so we have to call GetType

            var dynamicMethod = DynamicMethod.Create<AnyGetType<T>>($"Any_{type.Alias}_GetType");
            var generator = dynamicMethod.GetILGenerator();

            // Do we have to emit a null check?
            if (!type.IsValueType)
            {
                Label lblNotNull = generator.DefineLabel();
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldind_Ref);
                generator.Emit(OpCodes.Brtrue, lblNotNull);

                // instance is null, return typeof(T)
                generator.Emit(OpCodes.Ldtoken, type);
                generator.Emit(OpCodes.Call, _typeGetTypeFromHandleMethod);
                generator.Emit(OpCodes.Ret);

                // instance is not null, continue
                generator.MarkLabel(lblNotNull);
            }

            // call `instance.GetType() -> Type`
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Constrained, type);
            generator.Emit(OpCodes.Callvirt, _objectGetTypeMethod);
            generator.Emit(OpCodes.Ret);

            return dynamicMethod.CreateDelegate();
        }
    }
#endif
}