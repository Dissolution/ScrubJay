// ReSharper disable MethodOverloadWithOptionalParameter

#if NET9_0_OR_GREATER
using InlineIL;
using static InlineIL.IL;
#endif

namespace ScrubJay.Universal;

partial class Any
{
    /// <summary>
    /// Try to box the given <typeparamref name="T"/> <paramref name="value"/> into an <see cref="object"/>.
    /// </summary>
    /// <param name="value">
    /// The <typeparamref name="T"/> value to box.
    /// </param>
    /// <param name="boxed">
    /// The <see cref="object"/> that <paramref name="value"/> was boxed into.
    /// </param>
    /// <typeparam name="T">
    /// The <see cref="Type"/> of <paramref name="value"/> to attempt to box.
    /// </typeparam>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> was boxed,<br/>
    /// <see langword="false"/> if it was not.
    /// </returns>
    /// <remarks>
    /// This will always succeed for non-<c>ref struct</c> values.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryBox<T>(T? value, [NotNullIfNotNull(nameof(value))] out object? boxed)
    {
        boxed = (object?)value;
        return true;
    }
}

#if NET9_0_OR_GREATER
partial class Any
{
    /* store the FastBox<T> method here and not in MethodCache<T>,
     * as MethodCache<T> would fail compilation for any T : ref struct values.
     * Here we can abuse a compiler trick to ensure that only non-ref-struct Ts are ever boxed.
     */
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static object FastBox<T>(T value)
        where T : allows ref struct //, but _never_ will be
    {
        Emit.Ldarg(nameof(value));
        Emit.Box<T>();
        return Return<object>();
    }

    /// <summary>
    /// Try to box the given <typeparamref name="T"/> <paramref name="value"/> into an <see cref="object"/>.
    /// </summary>
    /// <param name="value">
    /// The <typeparamref name="T"/> value to box.
    /// </param>
    /// <param name="boxed">
    /// The <see cref="object"/> that <paramref name="value"/> was boxed into.
    /// </param>
    /// <param name="_">
    /// Ignored <see cref="TypeConstraints"/> on <typeparamref name="T"/> that assists with method overload resolution.
    /// </param>
    /// <typeparam name="T">
    /// The <see cref="Type"/> of <paramref name="value"/> to attempt to box.
    /// </typeparam>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> was boxed,<br/>
    /// <see langword="false"/> if it was not.
    /// </returns>
    /// <remarks>
    /// This will always succeed for non-<c>ref struct</c> values and will always fail for <c>ref struct</c> values.
    /// </remarks>
    public static bool TryBox<T>(T? value, out object? boxed, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        // ref structs cannot be boxed
        if (typeof(T).IsByRefLike)
        {
            boxed = null;
            return false;
        }

        /* Implementing this method is tricky. Even though we know that value is not a ref struct, the compiler doesn't.
         * Cannot use:
         *   `boxed = (object?)value`
         * Compilation fails with: `Cannot cast expression of type 'T' to type 'object?'`
         *
         * Cannot emit the box directly with:
         *   Emit.Ldarg(nameof(boxed));
         *   Emit.Ldarg(nameof(value));
         *   Emit.Box(typeof(T));
         *   Emit.Stind_Ref();
         *   Emit.Ldc_I4_1();
         *   Emit.Ret();
         *   throw Unreachable();
         * We get a compile-time exception when we pass in a ref struct value:
         *   `System.InvalidProgramException: Common Language Runtime detected an invalid program.`
         *
         * I came up with a clever hack. We can still use Emit, but instead of boxing the value in this method,
         * we call another method that does the boxing.
         * That method may still require the `allows ref struct` constraint, but as it will never be called
         * with anything but a non-ref struct T, the compiler never sees an issue.
         */

        Emit.Ldarg(nameof(boxed)); // ref object boxed
        Emit.Ldarg(nameof(value)); // T value
        Emit.Call(new MethodRef(typeof(Any), nameof(FastBox)).MakeGenericMethod(typeof(T)));
        Emit.Stind_Ref(); // object -> boxed
        Emit.Ldc_I4_1(); // 1 == true
        Emit.Ret();
        throw Unreachable();
    }
}

#endif