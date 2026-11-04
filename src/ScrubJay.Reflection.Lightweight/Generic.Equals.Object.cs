// ReSharper disable StaticMemberInGenericType

// ReSharper disable MethodOverloadWithOptionalParameter

using ScrubJay.Reflection.Lightweight.Extensions;

namespace ScrubJay.Reflection.Lightweight;

public static partial class Generic
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
        return GenericEqualsObjectCache<T>.Invoke(in instance, other);
    }

    internal delegate bool GenericEqualsObject<T>(in T? value, object? other)
        where T : allows ref struct;

    internal static class GenericEqualsObjectCache<T>
        where T : allows ref struct
    {
        public static readonly GenericEqualsObject<T> Invoke = CreateDelegate();

        private static GenericEqualsObject<T> CreateDelegate()
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
                equalsObjectMethod = MethodCache.Object_Equals_Object;
            }

            var dynamicMethod = DynamicMethod.Create<GenericEqualsObject<T>>($"Any_{type.Alias}_Equals");
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

            return dynamicMethod.CreateDelegate();
        }
    }

#endif
}