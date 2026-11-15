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
    [UsedImplicitly]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static object UnsafeBox<T>(T value)
        where T : allows ref struct // same as below, but since this is never called, no version with a ref-struct T will be compiled, and thus the Box<T> will never complain!
    {
        Emit.Ldarg(nameof(value));
        Emit.Box<T>();
        return Return<object>();
    }

    [UsedImplicitly]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static object BoxShim<T>(T value)
        where T : allows ref struct // this constraint has to exist to get past the call from TryBox, but this method will never be passed a ref-struct T value.
    {
        Emit.Ldarg(nameof(value));
        Emit.Call(new MethodRef(typeof(Anything), nameof(UnsafeBox)).MakeGenericMethod(typeof(T)));
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
        Emit.Call(new MethodRef(typeof(Anything), nameof(BoxShim)).MakeGenericMethod(typeof(T)));
        Emit.Stind_Ref();
        Emit.Ldc_I4_1();
        Emit.Ret();
        throw Unreachable();
    }
#endif

#endregion

#region Box

    // re-interpret a `ref T` into a `ref object skipping all checks (not a box, we 'know' that T is a non-value type)
    [UsedImplicitly]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ref object? AsObject<T>(ref T value)
#if NET9_0_OR_GREATER
        where T : allows ref struct // but never will be
#endif
    {
        return ref Unsafe.As<T, object?>(ref value);
    }

#if NET9_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static object? BoxNullable<T>(ref T value)
        where T : allows ref struct
    {
        return RuntimeHelpers.Box(ref Unsafe.As<T, byte>(ref value), typeof(T).TypeHandle);
    }
#endif

    [OverloadResolutionPriority(100)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNullIfNotNull(nameof(instance))]
    public static object? BoxedOrDefault<T>(T? instance)
    {
        return (object?)instance;
    }

#if NET9_0_OR_GREATER
    [OverloadResolutionPriority(75)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNullIfNotNull(nameof(instance))]
    public static object? BoxedOrDefault<T>(T? instance, Constrain.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        if (TryBox<T>(instance, out var boxed))
            return boxed;
        return null;
    }

#endif

#endregion

#endregion

#region Unboxing

#region CanUnbox

    public static bool CanUnboxAs(object? boxed, Type? type)
    {
        if (type is null)
            return boxed is null;
        return type.IsInstanceOfType(boxed);
    }

    public static bool CanUnboxAs<T>(object? boxed)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        return boxed is T;
    }

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

#endregion
}