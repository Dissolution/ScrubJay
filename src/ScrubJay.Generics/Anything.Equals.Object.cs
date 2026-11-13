// ReSharper disable StaticMemberInGenericType

// ReSharper disable MethodOverloadWithOptionalParameter

using System.Reflection;
using System.Reflection.Emit;
using ScrubJay.Generics.Extensions;

namespace ScrubJay.Generics;

public static partial class Anything
{
    [OverloadResolutionPriority(100)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equals<T>(T? instance, object? other)
    {
        if (instance is null)
            return other is null;
        return instance.Equals(other);
    }

#if NET9_0_OR_GREATER

    [OverloadResolutionPriority(90)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equals<T>(in T? instance, object? other)
        where T : allows ref struct
    {
        return AnythingEqualsObjectCache<T>.Invoke(in instance, other);
    }

    [OverloadResolutionPriority(90)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equals<T>(T? instance, object? other, Constrain.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        return AnythingEqualsObjectCache<T>.Invoke(in instance, other);
    }


    internal delegate bool AnythingEqualsObject<T>(in T? value, object? other)
        where T : allows ref struct;

    internal static class AnythingEqualsObjectCache<T>
        where T : allows ref struct
    {
        public static readonly AnythingEqualsObject<T> Invoke = CreateDelegate();

        private static AnythingEqualsObject<T> CreateDelegate()
        {
            var type = typeof(T);

            MethodInfo? equalsObjectMethod;

            // ref structs have to have a manual override
            if (type.IsByRefLike)
            {
                equalsObjectMethod = Reflect.FindInstanceMethod(
                    type,
                    nameof(object.Equals),
                    typeof(bool),
                    [typeof(object)]);

                if (equalsObjectMethod is null)
                {
                    // by default, ref structs do not equal any object
                    return static (in _, _) => false;
                }
            }
            else
            {
                equalsObjectMethod = Reflect.Object_Equals_Object;
            }

            var dynamicMethod = Reflect.CreateDynamicMethod<AnythingEqualsObject<T>>($"Anything_Equals_{type.Alias}");
            var generator = dynamicMethod.GetILGenerator();

            // Do we have to emit a null check?
            if (!type.IsValueType)
            {
                Label lblNotNull = generator.DefineLabel();
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldind_Ref);
                generator.Emit(OpCodes.Brtrue, lblNotNull);

                // instance is null, object must also be null
                generator.Emit(OpCodes.Ldarg_1);
                generator.Emit(OpCodes.Ldnull);
                generator.Emit(OpCodes.Ceq);
                generator.Emit(OpCodes.Ret);

                // instance is not null, continue
                generator.MarkLabel(lblNotNull);
            }

            // call `instance.Equals(object) -> bool`
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.Constrained, type);
            generator.Emit(OpCodes.Callvirt, equalsObjectMethod);
            generator.Emit(OpCodes.Ret);

            return dynamicMethod.CreateDelegate<AnythingEqualsObject<T>>();
        }
    }

#endif
}