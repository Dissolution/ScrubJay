// ReSharper disable StaticMemberInGenericType

// ReSharper disable MethodOverloadWithOptionalParameter

using System.Reflection.Emit;
using ScrubJay.Universal.Extensions;

namespace ScrubJay.Universal;

public static partial class Anything
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
        return AnythingGetTypeCache<T>.Invoke(in value);
    }

    internal delegate Type AnythingGetType<T>(in T? value)
        where T : allows ref struct;

    internal static class AnythingGetTypeCache<T>
        where T : allows ref struct
    {
        public static readonly AnythingGetType<T> Invoke = CreateDelegate();

        private static AnythingGetType<T> CreateDelegate()
        {
            var type = typeof(T);

            // some types are already basic
            if (type.IsValueType || type.IsByRefLike || type.IsByRef || type.IsPointer)
            {
                return (in _) => type;
            }

            // An instance of T may be more or less specific than T, so we have to call GetType

            var dynamicMethod = Reflect.CreateDynamicMethod<AnythingGetType<T>>($"Anything_GetType_{type.Alias}");
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
                generator.Emit(OpCodes.Call, Reflect.Type_GetTypeFromHandle);
                generator.Emit(OpCodes.Ret);

                // instance is not null, continue
                generator.MarkLabel(lblNotNull);
            }

            // call `instance.GetType() -> Type`
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Constrained, type);
            generator.Emit(OpCodes.Callvirt, Reflect.Object_GetType);
            generator.Emit(OpCodes.Ret);

            return dynamicMethod.CreateDelegate<AnythingGetType<T>>();
        }
    }
#endif
}