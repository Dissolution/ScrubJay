// ReSharper disable MethodOverloadWithOptionalParameter

using InlineIL;
using static InlineIL.IL;

namespace ScrubJay.Universal;

#if NET9_0_OR_GREATER
static partial class Any
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int CompareTo<T>(T? value, T? other, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        return Any<T>.CompareTo(value, other);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equals<T>(T? value, object? obj, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        return Any<T>.Equals(value, obj);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equals<T>(T? value, T? other, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        return Any<T>.Equals(value, other);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetHashCode<T>(T? value, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        return Any<T>.GetHashCode(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNullIfNotNull("value")]
    public static Type? GetType<T>(T? value, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        return Any<T>.GetType(value);
    }


    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static object FastBox<T>(T value)
        where T : allows ref struct // but _never_ will be
    {
        // this method does the actual boxing
        Emit.Ldarg(nameof(value));
        Emit.Box<T>();
        return Return<object>();
    }


    public static bool TryBox<T>(T? value, out object? boxed, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        if (value is null)
        {
            boxed = null;
            return true;
        }

        var type = typeof(T);

        // ref structs cannot be boxed
        if (type.IsByRefLike)
        {
            boxed = null;
            return false;
        }

        /* Cannot do `boxed = (object?)value` -- does not compile:
         *   Cannot cast expression of type 'T' to type 'object?'
         *
         * If we try to emit the box directly:
         *   Emit.Ldarg(nameof(boxed));
         *   Emit.Ldarg(nameof(value));
         *   Emit.Box(typeof(T));
         *   Emit.Stind_Ref();
         *   Emit.Ldc_I4_1();
         *   Emit.Ret();
         *   throw Unreachable();
         * We get a compile-time exception when we pass in a ref-struct for T:
         *   System.InvalidProgramException: Common Language Runtime detected an invalid program.
         *
         * There is a clever hack:
         *   Using indirection, we can call another method that has the IL, and we know that no version
         *   of that method will ever be compiled with a ref-struct for T, as we've eliminated them by this point.
         */

        Emit.Ldarg(nameof(boxed));  // ref object boxed
        Emit.Ldarg(nameof(value));  // T value
        Emit.Call(new MethodRef(typeof(Any), nameof(FastBox)).MakeGenericMethod(typeof(T)));
        Emit.Stind_Ref();           // object -> boxed
        Emit.Ldc_I4_1();            // 1 == true
        Emit.Ret();
        throw Unreachable();
    }
}
#endif