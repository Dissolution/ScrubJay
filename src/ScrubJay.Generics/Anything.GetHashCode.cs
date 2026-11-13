using System.Reflection;
using System.Reflection.Emit;
using ScrubJay.Generics.Extensions;

namespace ScrubJay.Generics;

public static partial class Anything
{
    [OverloadResolutionPriority(100)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetHashCode<T>(T? value)
    {
        if (value is null)
            return 0;
        return value.GetHashCode();
    }

#if NET9_0_OR_GREATER

    [OverloadResolutionPriority(90)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetHashCode<T>(in T? value)
        where T : allows ref struct
    {
        return AnythingGetHashCodeCache<T>.Invoke(in value);
    }

    internal delegate int AnythingGetHashCode<T>(in T? value)
        where T : allows ref struct;

    internal static class AnythingGetHashCodeCache<T>
        where T : allows ref struct
    {
        public static readonly AnythingGetHashCode<T> Invoke = CreateDelegate();

        private static AnythingGetHashCode<T> CreateDelegate()
        {
            var type = typeof(T);

            MethodInfo getHashCodeMethod;

            // ref structs can't normally be used with hashtables
            if (type.IsByRefLike)
            {
                // if they override GetHashCode, we'll use it
                // BEWARE: Span<T> and ReadOnlySpan<T> both throw an exception if GetHashCode is called!
                var method = Reflect.FindInstanceMethod(type, nameof(object.GetHashCode), typeof(int), []);
                if (method is null)
                {
                    // otherwise, we will hack and use the Address
                    return static (in instance) =>
                    {
                        unsafe
                        {
#if !NET10_0_OR_GREATER
                            ulong addr = (nuint)Unsafe.AsPointer<T>(ref Unsafe.AsRef<T>(in instance!));
#else
                            ulong addr = (nuint)Unsafe.AsPointer<T>(in instance!);
#endif
                            return (int)((addr >> 32) ^ addr);
                        }
                    };
                }

                getHashCodeMethod = method;
            }
            else
            {
                getHashCodeMethod = Reflect.Object_GetHashCode;
            }

            var dynamicMethod = Reflect.CreateDynamicMethod<AnythingGetHashCode<T>>($"Anything_GetHashCode_{type.Alias}");
            var generator = dynamicMethod.GetILGenerator();

            // Do we have to emit a null check?
            if (!type.IsValueType)
            {
                Label lblNotNull = generator.DefineLabel();
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldind_Ref);
                generator.Emit(OpCodes.Brtrue, lblNotNull);

                // instance is null, return 0
                generator.Emit(OpCodes.Ldc_I4_0);
                generator.Emit(OpCodes.Ret);

                // instance is not null, continue
                generator.MarkLabel(lblNotNull);
            }

            // call `instance.GetHashCode() -> int`
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Constrained, type);
            generator.Emit(OpCodes.Callvirt, getHashCodeMethod);
            generator.Emit(OpCodes.Ret);

            return dynamicMethod.CreateDelegate<AnythingGetHashCode<T>>();
        }
    }
#endif
}