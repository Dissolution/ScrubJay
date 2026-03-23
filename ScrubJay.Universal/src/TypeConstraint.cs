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
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/generics/constraints-on-type-parameters"/>
[PublicAPI]
public static class TypeConstraints
{
    /// <summary>
    /// Constrains <typeparamref name="T"/> to be a non-nullable value type,
    /// which includes <see langword="record"/> <see langword="struct"/> types.
    /// </summary>
    /// <remarks>
    /// Because all value types have an accessible parameterless constructor, either declared or implicit, the <see cref="IsStruct{T}"/> constraint implies the <see cref="HasNew{T}"/> constraint and can't be combined with it.<br/>
    /// You can't combine the <see cref="IsStruct{T}"/> constraint with the <see cref="IsUnmanaged{T}"/> constraint.
    /// </remarks>
    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct IsStruct<T>
        where T : struct;

    /// <summary>
    /// Constrains <typeparamref name="T"/> to be a reference type, either nullable or non-nullable.<br/>
    /// This constraint applies also to any <see langword="class"/>, <see langword="interface"/>, <see cref="Delegate"/>, or <see cref="Array"/> type, including <see langword="record">records</see>.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct IsNonNullClass<T>
        where T : class;

    /// <summary>
    /// Constrains <typeparamref name="T"/> to be a reference type.<br/>
    /// This constraint applies also to any <see langword="class"/>, <see langword="interface"/>, <see cref="Delegate"/>, or <see cref="Array"/> type.<br/>
    /// In a nullable context, <typeparamref name="T"/> must be a non-nullable reference type.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct IsNullableClass<T>
        where T : class;

    /// <summary>
    /// Constrains <typeparamref name="T"/> to be a non-nullable type.<br/>
    /// The argument can be a non-nullable reference type or a non-nullable value type.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct IsNotNull<T>
        where T : notnull;

    /// <summary>
    /// Constrains <typeparamref name="T"/> to be a non-nullable unmanaged type.
    /// </summary>
    /// <remarks>
    /// The <see cref="IsUnmanaged{T}"/> constraint implies the <see cref="IsStruct{T}"/> constraint and can't be combined with either the <see cref="IsStruct{T}"/> constraint nor the <see cref="HasNew{T}"/> constraint.
    /// </remarks>
    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct IsUnmanaged<T>
        where T : unmanaged;

    /// <summary>
    /// Constrains <typeparamref name="T"/> to have a public parameterless constructor.
    /// </summary>
    /// <remarks>
    /// The <see cref="HasNew{T}"/> constraint can't be combined with the <see cref="IsStruct{T}"/> and <see cref="IsUnmanaged{T}"/> constraints.
    /// </remarks>
    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct HasNew<T>
        where T : new();

    /// <summary>
    /// Constrains <typeparamref name="T"/> to be, derive from, or implement the specified <typeparamref name="O"/> base class or interface.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct DerivesFrom<T, O>
        where T : O;

    /// <summary>
    /// This constraint resolves the ambiguity when you need to specify an unconstrained type parameter when you override a method or provide an explicit interface implementation.
    /// The default constraint implies the base method without either the <see cref="IsNullableClass{T}"/>, <see cref="IsNonNullClass{T}"/>, or <see cref="IsStruct{T}"/> constraint.
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-9.0/unconstrained-type-parameter-annotations#default-constraint"/>
    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct IsUnconstrained<T>;

    /// <summary>
    /// This anti-constraint declares that <typeparamref name="T"/> can be a <see langword="ref struct"/> type.<br/>
    /// The generic type or method must obey ref safety rules for any instance of <typeparamref name="T"/> because it might be a <see langword="ref struct"/>.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct AllowsRefStruct<T>
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    ;

    /// <summary>
    /// This constraint limits <typeparamref name="T"/> to <see cref="Delegate"/> types.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct IsDelegate<T>
        where T : Delegate;

    /// <summary>
    /// This constraint requires <typeparamref name="E"/> to be a <see langword="struct"/> and <see cref="Enum"/>.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct IsEnum<E>
        where E : struct, Enum;


    // Common Derived Types

    /// <summary>
    /// Constrains <typeparamref name="T"/> to <see cref="IDisposable"/>.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct HasIDisposable<T>
        where T : IDisposable;

    /// <summary>
    /// Constrains <typeparamref name="T"/> to <see cref="IEquatable{T}"/>.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct HasIEquatable<T>
        where T : IEquatable<T>;

    /// <summary>
    /// Constrains <typeparamref name="T"/> to <see cref="IComparable{T}"/>.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct HasIComparable<T>
        where T : IComparable<T>;


    // Net7.0+ types
    // supported on lower versions to allow attributes to just exist

#if !NET7_0_OR_GREATER
    /// <summary>
    /// Constrains <typeparamref name="T"/> to <see langword="ISpanParsable{T}"/>.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct HasISpanParsable<T>;

    /// <summary>
    /// Constrains <typeparamref name="T"/> to <see langword="INumberBase{T}"/>.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct HasINumberBase<T>;

    /// <summary>
    /// Constrains <typeparamref name="T"/> to <see langword="IEqualityOperators{T,T,bool}"/>.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct HasIEqualityOperators<T>;

    /// <summary>
    /// Constrains <typeparamref name="T"/> to <see langword="IComparisonOperators{T,T,int}"/>
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct HasIComparisonOperators<T>;

#else
    /// <summary>
    /// Constrains <typeparamref name="T"/> to <see cref="ISpanParsable{T}"/>.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct HasISpanParsable<T>
        where T : ISpanParsable<T>;

    /// <summary>
    /// Constrains <typeparamref name="T"/> to <see cref="INumberBase{T}"/>.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct HasINumberBase<T>
        where T : INumberBase<T>;

    /// <summary>
    /// Constrains <typeparamref name="T"/> to <see cref="IEqualityOperators{T,T,bool}"/>.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct HasIEqualityOperators<T>
        where T : IEqualityOperators<T, T, bool>;

    /// <summary>
    /// Constrains <typeparamref name="T"/> to
    /// <see cref="IComparisonOperators{T,T,int}"/>.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct HasIComparisonOperators<T>
        where T : IComparisonOperators<T, T, int>;

#endif




    // combinations

    /// <summary>
    /// Constrains <typeparamref name="T"/> to <see cref="IDisposable"/> and <c>new()</c>.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0)]
    public readonly struct IsDisposableNew<T>
        where T : IDisposable, new();


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