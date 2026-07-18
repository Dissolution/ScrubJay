// ReSharper disable MethodOverloadWithOptionalParameter
namespace ScrubJay.Universal;

[PublicAPI]
public static class Box
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryUnbox<T>(object? box, [MaybeNullWhen(false)] out T? value)
    {
        if (box is T unboxed)
        {
            value = unboxed;
            return true;
        }

        value = default;
        return false;
    }

#if NET9_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryUnbox<T>(
        object? box,
        [MaybeNullWhen(false)] out T? value,
        TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        if (box is T unboxed)
        {
            value = unboxed;
            return true;
        }

        value = default;
        return false;
    }
#endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ref T UnsafeUnboxRef<T>(object box)
    {
        Emit.Ldarg(nameof(box));
        Emit.Unbox<T>();
        return ref ReturnRef<T>();
    }

    [return: MaybeNull]
    public static ref T? TryUnboxRef<T>(object? box)
    {
        if (box is T)
        {
            return ref UnsafeUnboxRef<T>(box)!;
        }
        else
        {
            return ref Unsafe.NullRef<T?>();
        }
    }


    public static bool Contains<T>(object? box)
    {
        return box is T;
    }

#if NET9_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Contains<T>(object? box,
        TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        return box is T;
    }
#endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CanBox<T>(in T? value)
    {
        return true;
    }

#if NET9_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CanBox<T>(in T? value,
        TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        Type? type = Any.GetType<T>(in value);
        return !type.IsByRefLike;
    }
#endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static object? BoxOrNull<T>(T? value)
    {
        return (object?)value;
    }

#if NET9_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static object? BoxOrNull<T>(T? value,
        TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        if (TryBox<T>(value, out var box, _))
            return box;
        return null;
    }
#endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryBox<T>(
        T? value,
        [NotNullIfNotNull(nameof(value)), MaybeNullWhen(false)]
        out object? boxed)
    {
        boxed = (object?)value;
        return true;
    }

#if NET9_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static object UnsafeBox<T>(T value)
        where T : allows ref struct //, but _never_ will be
    {
        Emit.Ldarg(nameof(value));
        Emit.Box<T>();
        return Return<object>();
    }

    public static bool TryBox<T>(
        T? value,
        [NotNullIfNotNull(nameof(value)), MaybeNullWhen(false)]
        out object? boxed,
        TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        // ref structs cannot be boxed
        if (Any.GetType<T>(in value).IsByRefLike)
        {
            boxed = null;
            return false;
        }

        /* The rest of this method is tricky -- _We_ know that the value is not a ref struct, but the compiler does not.

           Fail 1 - The same code as TryBox:
           ```
           boxed = (object?)value;
           return true;
           ```
           - Compiler error: `Cannot cast expression of type 'T?' to type 'object?'`

           Fail 2 - The same IL as TryBox:
           ```
           Emit.Ldarg(nameof(boxed));
           Emit.Ldarg(nameof(value));
           Emit.Box(typeof(T));
           Emit.Stind_Ref();
           Emit.Ldc_I4_1();
           Emit.Ret();
           throw Unreachable();
           ```
           - Runtime error with `ref struct`: `System.InvalidProgramException: Common Language Runtime detected an invalid program.`

           Fail 3 - Just calling the TryBox:
           ```
           Emit.Ldarg(nameof(value));
           Emit.Ldarg(nameof(boxed));
           Emit.Call(MethodRef.Method(typeof(Any), nameof(TryBox), [typeof(T), typeof(object).MakeByRefType()]).MakeGenericMethod(typeof(T)));
           Emit.Ret();
           throw Unreachable();
           ```
           - Runtime error with `ref struct`: `System.Security.VerificationException: type argument 'XXX' violates the constraint of type parameter 'T'.

           After a bit of tinkering, I came up with a clever hack:
           We can still use `Emit` - instead of boxing the value in this method, we box the value in another method.
           That method can still have the `where T : allows ref struct` constraint, but as no instance of it with a `ref struct` T will ever
           be used (as it is gated behind the above check), it will never complain.
         */

        Emit.Ldarg(nameof(boxed));
        Emit.Ldarg(nameof(value));
        Emit.Call(new MethodRef(typeof(Box), nameof(Box.UnsafeBox)).MakeGenericMethod(typeof(T)));
        Emit.Stind_Ref();
        Emit.Ldc_I4_1();
        Emit.Ret();
        throw Unreachable();
    }
#endif
}