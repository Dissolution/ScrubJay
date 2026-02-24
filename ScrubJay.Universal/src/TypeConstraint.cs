#pragma warning disable S2326, CA1711

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
    /// <summary>
    /// Anti-constrains <typeparamref name="T"/> to <c>allow ref struct</c>.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct AllowsRefStruct<T>
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    ;

    /// <summary>
    /// Constrains <typeparamref name="T"/> to <see langword="unmanaged"/>.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct IsUnmanaged<T>
        where T : unmanaged;

    /// <summary>
    /// Constrains <typeparamref name="T"/> to <see langword="struct"/>.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct IsStruct<T>
        where T : struct;

    /// <summary>
    /// Constrains <typeparamref name="T"/> to <see langword="class"/>.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct IsClass<T>
        where T : class;

    /// <summary>
    /// Constrains <typeparamref name="E"/> to <see langword="struct"/> and <see cref="Enum"/>.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct IsEnum<E>
        where E : struct, Enum;

    /// <summary>
    /// Constrains <typeparamref name="T"/> to <c>new()</c>.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct IsNew<T>
        where T : new();

    /// <summary>
    /// Constrains <typeparamref name="T"/> to <see cref="IDisposable"/>.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct IsDisposable<T>
        where T : IDisposable;

    /// <summary>
    /// Constrains <typeparamref name="T"/> to <see cref="IDisposable"/> and <c>new()</c>.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct IsDisposableNew<T>
        where T : IDisposable, new();
    
    /// <summary>
    /// Constrains <typeparamref name="T"/> to <see cref="IEquatable{T}"/>.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct IsEquatable<T>
        where T : IEquatable<T>;

    /// <summary>
    /// Constrains <typeparamref name="T"/> to <see cref="IComparable{T}"/>.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct IsComparable<T>
        where T : IComparable<T>;

    /// <summary>
    /// Constrains <typeparamref name="T"/> to <see cref="ISpanParsable{T}"/>.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct IsSpanParsable<T>
#if NET7_0_OR_GREATER
        where T : ISpanParsable<T>
#endif
    ;

    /// <summary>
    /// Constrains <typeparamref name="T"/> to <see cref="INumberBase{T}"/>.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct IsNumberBase<T>
#if NET7_0_OR_GREATER
        where T : INumberBase<T>
#endif
    ;

    /// <summary>
    /// Constrains <typeparamref name="T"/> to <see langword="unmanaged"/> and <c>allows ref struct</c>.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct IsUnmanagedAllowsRefStruct<T>
        where T : unmanaged
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    ;
}