using ScrubJay.Reflection.Lightweight.Extensions;

namespace ScrubJay.Reflection.Lightweight;

public static partial class Generic
{
    [OverloadResolutionPriority(100)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string? ToString<T>(T? value)
    {
        if (value is null)
            return null;
        return value.ToString();
    }

#if NET9_0_OR_GREATER

    [OverloadResolutionPriority(90)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string? ToString<T>(in T? value)
        where T : allows ref struct
    {
        return AnyToStringCache<T>.Invoke(in value);
    }

    internal delegate string? AnyToString<T>(in T? value)
        where T : allows ref struct;

    internal static class AnyToStringCache<T>
        where T : allows ref struct
    {
        private static readonly MethodInfo _objectToStringMethod = typeof(object).GetMethod(nameof(ToString), BindingFlags.Public | BindingFlags.Instance)!;

        public static readonly AnyToString<T> Invoke = CreateDelegate();

        private static AnyToString<T> CreateDelegate()
        {
            var type = typeof(T);

            MethodInfo toStringMethod;

            if (type.IsByRefLike)
            {
                // ref structs have to override ToString directly to be called
                var tsm = Reflect.FindInstanceMethod(type, nameof(object.ToString), typeof(string), []);
                if (tsm is null)
                {
                    // From object.ToString:
                    // The default for an object is to return the fully qualified name of the class.
                    return static (in _) => typeof(T).ToString();
                }

                toStringMethod = tsm;
            }
            else
            {
                toStringMethod = _objectToStringMethod;
            }

            var dynamicMethod = DynamicMethod.Create<AnyToString<T>>($"Any_{type.Alias}_ToString");
            var generator = dynamicMethod.GetILGenerator();

            // Do we have to emit a null check?
            if (!type.IsValueType)
            {
                Label lblNotNull = generator.DefineLabel();
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldind_Ref);
                generator.Emit(OpCodes.Brtrue, lblNotNull);

                // instance is null, return null
                generator.Emit(OpCodes.Ldnull);
                generator.Emit(OpCodes.Ret);

                // instance is not null, continue
                generator.MarkLabel(lblNotNull);
            }

            // call `instance.ToString() -> string?`
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Constrained, type);
            generator.Emit(OpCodes.Callvirt, toStringMethod);
            generator.Emit(OpCodes.Ret);

            return dynamicMethod.CreateDelegate();
        }
    }
#endif
}