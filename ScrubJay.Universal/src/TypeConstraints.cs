namespace ScrubJay.Universal;

/// <summary>
/// Type Constraints are provided to allow generic methods with different <c>where</c> constraints to co-exist without compiler error.<br />
/// <br/>
/// This code will not compile:<code>
/// public T DoThing&lt;T&gt;(T value) where T : struct;
/// public T DoThing&lt;T&gt;(T value) where T : class;
/// </code>
/// Error: <i>member with the same signature is already declared</i> <br />
/// -------------<br/>
/// You can use <see cref="TypeConstraints"/> to fix the error:<br />
/// <code>
/// public static T DoThing&lt;T&gt;(T value, TypeConstraints.IsStruct&lt;T&gt; _ = default) where T : struct
/// public static T DoThing&lt;T&gt;(T value, TypeConstraints.IsClass&lt;T&gt; _ = default) where T : class
/// </code>
/// </summary>
[PublicAPI]
public static class TypeConstraints
{
    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct AllowsRefStruct<T>
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
        ;

    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct IsUnmanaged<T>
        where T : unmanaged;

    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct IsStruct<T>
        where T : struct;

    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct IsClass<T>
        where T : class;

    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct IsEnum<E>
        where E : struct, Enum;

    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct IsNew<T>
        where T : new();

    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct IsDisposable<T>
        where T : IDisposable;

    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct IsEquatable<T>
        where T : IEquatable<T>;

    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct IsComparable<T>
        where T : IComparable<T>;

    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct IsSpanParsable<T>
#if NET7_0_OR_GREATER
        where T : ISpanParsable<T>
#endif
        ;

    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct IsNumberBase<T>
#if NET7_0_OR_GREATER
        where T : INumberBase<T>
#endif
        ;


    public readonly struct IsUnmanagedAllowsRefStruct<T>
        where T : unmanaged
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    ;

}