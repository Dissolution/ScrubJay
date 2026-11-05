#pragma warning disable IDE0060

#if NET9_0_OR_GREATER
using InlineIL;
#endif
using ScrubJay.Universal.Extensions;
using static InlineIL.IL;

namespace ScrubJay.Universal;

public static partial class Anything
{
#region TryUnbox

    public static bool TryUnbox<T>(
        object? box,
        [MaybeNullWhen(false)] out T? value)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        if (box is T unboxed)
        {
            value = unboxed;
            return true;
        }

        value = default;
        return false;
    }

    [UsedImplicitly]
#pragma warning disable IDE0051
    private static ref T UnsafeUnboxRef<T>(object box)
#pragma warning restore IDE0051
    {
        Emit.Ldarg(nameof(box));
        Emit.Unbox<T>();
        return ref ReturnRef<T>();
    }

    [return: MaybeNull]
    public static ref T? TryUnboxRef<T>(object? box)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        Emit.Ldarg(nameof(box));
        Emit.Isinst<T>();
        Emit.Ldnull();
        Emit.Ceq();
        Emit.Brtrue("not_a_T");

        Emit.Ldarg(nameof(box));
        Emit.Unbox<T>();
        Emit.Ret();

        MarkLabel("not_a_T");
        Emit.Ldc_I4_0();
        Emit.Conv_U();
        Emit.Ret();

        throw Unreachable();
    }

#endregion

#region CanBox

    public static bool CanBox(Type? type)
    {
        return type is null || !type.IsByRefLike;
    }

    public static bool CanBox<T>(T? instance = default)
    {
        return instance is null || !typeof(T).IsByRefLike;
    }

#if NET9_0_OR_GREATER
    public static bool CanBox<T>(in T? instance = default)
        where T : allows ref struct
    {
        return CanBox(GetType<T>(in instance));
    }
#endif

#endregion

#region TryBox

    [OverloadResolutionPriority(100)]
    public static bool TryBox<T>(
        T? value,
        [NotNullIfNotNull(nameof(value)), MaybeNullWhen(false)]
        out object? boxed)
    {
        boxed = (object?)value;
        return true;
    }

#if NET9_0_OR_GREATER
    [UsedImplicitly]
    private static object UnsafeBoxImpl<T>(T value)
        where T : allows ref struct //, but _never_ will be
    {
        Emit.Ldarg(nameof(value));
        Emit.Box<T>();
        return Return<object>();
    }

    internal static object UnsafeBox<T>(T value)
        where T : allows ref struct
    {
        Emit.Ldarg(nameof(value));
        Emit.Call(new MethodRef(typeof(Anything), nameof(UnsafeBoxImpl)).MakeGenericMethod(typeof(T)));
        return Return<object>();
    }

    [OverloadResolutionPriority(90)]
    public static bool TryBox<T>(
        T? value,
        [NotNullIfNotNull(nameof(value)), MaybeNullWhen(false)]
        out object? boxed,
        // ReSharper disable once MethodOverloadWithOptionalParameter
        Constrain.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        // ref structs cannot be boxed
        if (GetType<T>(in value).IsByRefLike)
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
        Emit.Call(new MethodRef(typeof(Anything), nameof(UnsafeBoxImpl)).MakeGenericMethod(typeof(T)));
        Emit.Stind_Ref();
        Emit.Ldc_I4_1();
        Emit.Ret();
        throw Unreachable();
    }
#endif

#endregion
}