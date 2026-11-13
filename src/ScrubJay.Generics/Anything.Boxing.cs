// ReSharper disable MethodOverloadWithOptionalParameter

#pragma warning disable IDE0060

#if NET9_0_OR_GREATER
using InlineIL;
#endif
using static InlineIL.IL;


namespace ScrubJay.Generics;

public static partial class Anything
{
#region Boxing

#region CanBox

    /// <summary>
    /// Can instances of the given <see cref="Type"/> be boxed?
    /// </summary>
    /// <param name="type">The <see cref="Type"/> to examine.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="type"/> is <see langword="null"/> or a non-<see langword="static"/>, non-ref-struct <see cref="Type"/>.
    /// </returns>
    [OverloadResolutionPriority(100)]
    public static bool CanBox(Type? type)
    {
        return type is null or { IsStatic: false, IsByRefLike: false };
    }

    [OverloadResolutionPriority(75)]
    public static bool CanBox<T>(T? instance = default)
    {
        return instance is null || !typeof(T).IsByRefLike;
    }

#if NET9_0_OR_GREATER
    [OverloadResolutionPriority(50)]
    public static bool CanBox<T>(T? instance = default, Constrain.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        return !AnythingGetTypeCache<T>.Invoke(in instance).IsByRefLike;
    }
#endif

#endregion

#region TryBox

    [OverloadResolutionPriority(100)]
    public static bool TryBox<T>(
        T? instance,
        [NotNullIfNotNull(nameof(instance)), MaybeNullWhen(false)]
        out object? boxed)
    {
        boxed = (object?)instance;
        return true;
    }

#if NET9_0_OR_GREATER

    /*

     - Boxing, Unboxing, and ref-structs.

     The #1 issue with boxing and the `allows ref struct` anti-constraint is how the compiler treats those `T` values - like they are radioactive.

     If we want to support this signature:
     ```csharp
      public static bool TryBox<T>(T? instance, out object? boxed)
        where T : allows ref struct
     ```

     We have several issues. Even after we guard on T with something like:
     ```csharp
     if (typeof(T).IsByRefLike)
     {
        boxed = null;
        return false;
     }
     ```

     We have failures:

     - Fail #1

     If we just try to use the same code as `TryBox`:
     ```csharp
     boxed = (object?)value;
     return true;
     ```

     There is a compiler error: `Cannot cast expression of type 'T?' to type 'object?'`

     - Fail #2

     If we just try to use `InlineIL.Emit` to emit that code:
     ```csharp
     Emit.Ldarg(nameof(boxed));
     Emit.Ldarg(nameof(value));
     Emit.Box(typeof(T));
     Emit.Stind_Ref();
     Emit.Ldc_I4_1();
     Emit.Ret();
     throw Unreachable();
     ```

     There is instead a _runtime_ error whenever any `ref struct` is used as a `T`: `System.InvalidProgramException: Common Language Runtime detected an invalid program.`

     - Fail #3

     If we use a guard and then try to call `TryBox`:
     ```csharp

     ```

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

    [UsedImplicitly]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static object UnsafeBoxImpl<T>(T value)
        where T : allows ref struct //, but _never_ will be
    {
        Emit.Ldarg(nameof(value));
        Emit.Box<T>();
        return Return<object>();
    }

    [UsedImplicitly]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        Constrain.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        if (typeof(T).IsByRefLike)
        {
            boxed = null;
            return false;
        }

        Emit.Ldarg(nameof(boxed));
        Emit.Ldarg(nameof(value));
        Emit.Box(typeof(T));
        Emit.Stind_Ref();
        Emit.Ldc_I4_1();
        Emit.Ret();
        throw Unreachable();


        //// ref structs cannot be boxed
        //if (GetType<T>(in value).IsByRefLike)
        //{
        //    boxed = null;
        //    return false;
        //}
//
        //Emit.Ldarg(nameof(boxed));
        //Emit.Ldarg(nameof(value));
        //Emit.Call(new MethodRef(typeof(Anything), nameof(UnsafeBoxImpl)).MakeGenericMethod(typeof(T)));
        //Emit.Stind_Ref();
        //Emit.Ldc_I4_1();
        //Emit.Ret();
        //throw Unreachable();
    }
#endif

#endregion

#region Box

    // re-interpret a `ref T` into a `ref object skipping all checks (not a box, we 'know' that T is a non-value type)
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ref object? AsObject<T>(ref T value)
#if NET9_0_OR_GREATER
        where T : allows ref struct // but never will be
#endif
    {
        return ref Unsafe.As<T, object?>(ref value);
    }

#endregion

#endregion


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
}