using static InlineIL.IL;
using InlineIL;

namespace ScrubJay.Universal;

[PublicAPI]
public static partial class Generic
{
    [OverloadResolutionPriority(100)]
    public static bool Is<V, T>(V value)
    {
        return value is T;
    }

    [OverloadResolutionPriority(100)]
    public static bool Is<V, T>(V value, [MaybeNullWhen(false)] out T typed)
    {
        if (value is T isType)
        {
            typed = isType;
            return true;
        }

        typed = default;
        return false;
    }

#if NET9_0_OR_GREATER
    private static bool UnsafeIs<V, T>(V value)
        where V : allows ref struct
        where T : allows ref struct
    {
        Emit.Ldarg(nameof(value));
        Emit.Box<V>();
        Emit.Isinst<T>();
        Emit.Ldnull();
        Emit.Cgt_Un();
        return Return<bool>();
    }

    [OverloadResolutionPriority(50)]
    public static bool Is<V, T>(V value, Constrain.AllowsRefStruct<V> _ = default)
        where V : allows ref struct
        where T : allows ref struct
    {
        if (typeof(V) == typeof(T))
            return true;
        if (typeof(V).IsByRefLike || typeof(T).IsByRefLike)
            return false;
        Emit.Ldarg(nameof(value));
        Emit.Call(MethodRef.Method(typeof(Generic), nameof(UnsafeIs)).MakeGenericMethod(typeof(V), typeof(T)));
        return Return<bool>();
    }

    private static bool UnsafeIsOut<V, T>(V value, out T typed)
        where V : allows ref struct
        where T : allows ref struct
    {
        DeclareLocals(new LocalVar("isType", typeof(T)));

        Emit.Ldarg(nameof(value));
        Emit.Box<V>();
        Emit.Isinst<T>();
        Emit.Brfalse("not_a_T");

        Emit.Ldarg(nameof(value));
        Emit.Box<V>();
        Emit.Isinst<T>();
        Emit.Unbox_Any<T>();
        Emit.Stloc("isType");

        Emit.Ldarg(nameof(typed));
        Emit.Ldloc("isType");
        Emit.Stobj<T>();
        Emit.Ldc_I4_1();
        Emit.Ret();

        MarkLabel("not_a_T");
        Emit.Ldarg(nameof(typed));
        Emit.Initobj<T>();
        Emit.Ldc_I4_0();
        Emit.Ret();

        throw Unreachable();
    }

    [OverloadResolutionPriority(50)]
    public static bool Is<V, T>(V value, [MaybeNullWhen(false)] out T typed,
        Constrain.AllowsRefStruct<V> _ = default)
        where V : allows ref struct
        where T : allows ref struct
    {
        if (typeof(V) == typeof(T))
        {
            typed = Unsafe.As<V, T>(ref value);
            return true;
        }
        if (typeof(V).IsByRefLike || typeof(T).IsByRefLike)
        {
            typed = default;
            return false;
        }
        Emit.Ldarg(nameof(value));
        Emit.Ldarg(nameof(typed));
        Emit.Call(MethodRef.Method(typeof(Generic), nameof(UnsafeIsOut)).MakeGenericMethod(typeof(V), typeof(T)));
        Emit.Ret();
        throw Unreachable();
    }
#endif
}