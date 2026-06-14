// ReSharper disable MethodOverloadWithOptionalParameter

#pragma warning disable IDE0060

namespace ScrubJay.Universal;

public partial class Any
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNullIfNotNull(nameof(value))]
    public static object? BoxOr<T>(T? value, object? _)
    {
        return (object?)value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNullIfNotNull(nameof(value))]
    public static object? BoxOrToString<T>(in T? value)
    {
        return (object?)value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryBox<T>(T? value, [NotNullIfNotNull(nameof(value))] out object? boxed)
    {
        boxed = (object?)value;
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static object FastBox<T>(T value)
#if NET9_0_OR_GREATER
        where T : allows ref struct //, but _never_ will be
#endif
    {
        Emit.Ldarg(nameof(value));
        Emit.Box<T>();
        return Return<object>();
    }

    // ReSharper disable once MethodOverloadWithOptionalParameter
    public static bool TryBox<T>(T? value, out object? boxed, TypeConstraints.AllowsRefStruct<T> _ = default)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        // ref structs cannot be boxed
        if (typeof(T).IsByRefLike)
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
        Emit.Call(new MethodRef(typeof(Any), nameof(FastBox)).MakeGenericMethod(typeof(T)));
        Emit.Stind_Ref();
        Emit.Ldc_I4_1();
        Emit.Ret();
        throw Unreachable();
    }

#if NET9_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNullIfNotNull(nameof(value))]
    public static object? BoxOr<T>(T? value, object? fallback, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        if (TryBox<T>(value, out var boxed, _))
            return boxed;
        return fallback;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNullIfNotNull(nameof(value))]
    public static object? BoxOrToString<T>(in T? value, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        if (TryBox<T>(value, out var boxed, _))
            return boxed;
        return (object?)Any.ToString<T>(in value, _);
    }
#endif


    [return: NotNullIfNotNull(nameof(value))]
    public static object? BoxOrBytes<T>(in T? value)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        if (value is null)
            return null;
        if (!TryBox<T>(value, out object? boxed))
            boxed = (object)GetReferenceBytes<T>(in value).ToArray();
        return boxed!;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryUnbox<T>(object? box, [MaybeNullWhen(false)] out T value)
    {
        // ReSharper disable once MergeCastWithTypeCheck
        if (box is T)
        {
            value = (T)box;
            return true;
        }
        value = default;
        return false;
    }
}