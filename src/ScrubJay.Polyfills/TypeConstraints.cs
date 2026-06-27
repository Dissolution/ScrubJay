namespace ScrubJay.Polyfills;

/// <summary>
/// This class contains marker <see langword="struct">structs</see> that themselves carry type parameter constraints.<br/>
/// These are used to disambiguate method overloads that cannot normally co-exist with only their type parameter constraints.
/// </summary>
/// <remarks>
/// <para>
/// The C# compiler does not use generic type constraints when differentiating methods:
/// <code>
/// public void FooBar&lt;T&gt;(T value) where T : struct;
/// public void FooBar&lt;T&gt;(T value) where T : class;
/// </code>
/// ^ This code will cause error <see href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-messages/overload-resolution#duplicate-overloads-defined">CS0111</see>,
/// <i>Type already defines a member called 'FooBar' with the same parameter types</i>
/// </para>
/// <para>
/// Adding a <see cref="TypeConstraints"/> parameter with a <see langword="default"/> can solve this problem:
/// <code>
/// public static T DoThing&lt;T&gt;(T value, TypeConstraints.IsStruct&lt;T&gt; _ = default) where T : struct;
/// public static T DoThing&lt;T&gt;(T value, TypeConstraints.IsClass&lt;T&gt; _ = default) where T : class;
/// </code>
/// <i>The caller never passes the TypeConstraint explicitly; it exists for the compiler.</i>
/// </para>
/// </remarks>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/generics/constraints-on-type-parameters"/>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/where-generic-type-constraint"/>
[PublicAPI]
public static class TypeConstraints
{
#region Type Parameter Constraints
    /// <summary>
    /// Type argument <typeparamref name="T"/> has no constraints.
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/generics/constraints-on-type-parameters#unbounded-type-parameters"/>
    [StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
    public readonly struct Unbounded<T>;

    /// <summary>
    /// Type argument <typeparamref name="T"/> must be a non-nullable value type, which includes <see langword="record struct"/> types.
    /// </summary>
    /// <remarks>
    /// Because all value types have an accessible parameterless constructor, either declared or implicit,
    /// the <see cref="IsStruct{T}"/> constraint implies the <see cref="HasNew{T}"/> constraint and can't be combined with it.<br/>
    /// You can't combine the <see cref="IsStruct{T}"/> constraint with the <see cref="IsUnmanaged{T}"/> constraint.
    /// </remarks>
    [StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
    public readonly struct IsStruct<T>
        where T : struct;

    /// <summary>
    /// Type argument <typeparamref name="T"/> must be a reference type.<br/>
    /// This constraint applies also to any <see langword="class"/>, <see langword="interface"/>, <see cref="Delegate"/>, or <see cref="Array"/> type.
    /// </summary>
    /// <remarks>
    /// In a nullable context, <typeparamref name="T"/> must be a non-nullable reference type.
    /// </remarks>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/generics/constraints-on-type-parameters#class-constraint"/>
    [StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
    public readonly struct IsClass<T>
        where T : class;

    /// <summary>
    /// Type argument <typeparamref name="T"/> must be a reference type, either nullable or non-nullable.<br/>
    /// This constraint applies also to any <see langword="class"/>, <see langword="interface"/>, <see cref="Delegate"/>, or <see cref="Array"/> type.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
    public readonly struct IsNullableClass<T>
        where T : class?;

    /// <summary>
    /// Type argument <typeparamref name="T"/> must be a non-nullable type.<br/>
    /// The argument can be a non-nullable reference type or a non-nullable value type.
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/generics/constraints-on-type-parameters#notnull-constraint"/>
    [StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
    public readonly struct IsNotNull<T>
        where T : notnull;

    /// <summary>
    /// Type argument <typeparamref name="T"/> must be a non-nullable <see langword="unmanaged"/> type.
    /// </summary>
    /// <remarks>
    /// The <see cref="IsUnmanaged{T}"/> constraint implies the <see cref="IsStruct{T}"/> constraint and can't be combined with either it nor the <see cref="HasNew{T}"/> constraint.
    /// </remarks>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/generics/constraints-on-type-parameters#unmanaged-constraint"/>
    [StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
    public readonly struct IsUnmanaged<T>
        where T : unmanaged;

    /// <summary>
    /// Type argument <typeparamref name="T"/> must have a <see langword="public"/> parameterless constructor.
    /// </summary>
    /// <remarks>
    /// The <see cref="HasNew{T}"/> constraint can't be combined with either the <see cref="IsStruct{T}"/> and <see cref="IsUnmanaged{T}"/> constraints.
    /// </remarks>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/new-constraint"/>
    [StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
    public readonly struct HasNew<T>
        where T : new();

    /// <summary>
    /// Type argument <typeparamref name="T"/> must be or derive from the specified <typeparamref name="TBase"/> <see langword="class"/>.
    /// </summary>
    /// <remarks>
    /// In a nullable context, <typeparamref name="T"/> must be a non-nullable reference type derived from the specified base <see langword="class"/>.
    /// </remarks>
    public readonly struct HasBaseClass<T, TBase>
        where T : class, TBase
        where TBase : class;

    /// <summary>
    /// Type argument <typeparamref name="T"/> must be or derive from the specified <typeparamref name="TBase"/> <see langword="class"/>.
    /// </summary>
    /// <remarks>
    /// In a nullable context, <typeparamref name="T"/> can be either a nullable or non-nullable type derived from the specified base <see langword="class"/>.
    /// </remarks>
    public readonly struct HasNullableBaseClass<T, TBase>
        where T : class?, TBase
        where TBase : class?;

    /// <summary>
    /// Type argument <typeparamref name="T"/> must be or implement the specified <typeparamref name="TInterface"/> <see langword="interface"/>.<br/>
    /// Multiple <see langword="interface"/> constraints can be specified.<br/>
    /// The constraining <see langword="interface">interfaces</see> can also be generic.
    /// </summary>
    /// <remarks>
    /// In a nullable context, <typeparamref name="T"/> must be a non-nullable type that implements the specified <typeparamref name="TInterface"/> <see langword="interface"/>.
    /// </remarks>
    public readonly struct HasInterface<T, TInterface>
        where T : TInterface;

    /// <summary>
    /// Type argument <typeparamref name="T"/> must be or implement the specified <typeparamref name="TInterface"/> <see langword="interface"/>.<br/>
    /// Multiple <see langword="interface"/> constraints can be specified.<br/>
    /// The constraining <see langword="interface">interfaces</see> can also be generic.
    /// </summary>
    /// <remarks>
    /// In a nullable context, <typeparamref name="T"/> can be a nullable reference type, a non-nullable reference type, or a value type.<br/>
    /// <typeparamref name="T"/> can't be a nullable value type.
    /// </remarks>
    public readonly struct HasNullableInterface<T, TInterface>
        where T : TInterface?;

    /// <summary>
    /// Type argument <typeparamref name="T"/> must be or derive from the <typeparamref name="TUnderlying"/> type.
    /// </summary>
    /// <remarks>
    /// In a nullable context, if <typeparamref name="TUnderlying"/> is a non-nullable reference type, <typeparamref name="T"/> must be a non-nullable reference type.<br/>
    /// If <typeparamref name="TUnderlying"/> is a nullable reference type, <typeparamref name="T"/> can be either nullable or non-nullable.
    /// </remarks>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/generics/constraints-on-type-parameters#type-parameters-as-constraints"/>
    [StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
    public readonly struct DerivesFrom<T, TUnderlying>
        where T : TUnderlying;

    /// <summary>
    /// This constraint resolves the ambiguity when you need to specify an unconstrained type parameter when you override a method or provide an explicit interface implementation.
    /// </summary>
    /// <remarks>
    /// The default constraint implies the base method without either the <see cref="IsClass{T}"/> or <see cref="IsStruct{T}"/> constraint.
    /// </remarks>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-9.0/unconstrained-type-parameter-annotations#default-constraint"/>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/generics/constraints-on-type-parameters#default-constraint"/>
    [StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
    public readonly struct IsUnconstrained<T>
        // where T : default
        ;

#if NET9_0_OR_GREATER
    /// <summary>
    /// This anti-constraint declares that <typeparamref name="T"/> can be a <see langword="ref struct"/> type.<br/>
    /// The generic type or method must obey ref safety rules for any instance of <typeparamref name="T"/> because it might be a <see langword="ref struct"/>.
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/generics/constraints-on-type-parameters#allows-ref-struct"/>
    [StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
    public readonly struct AllowsRefStruct<T>
        where T : allows ref struct;
#endif
#endregion / Type Parameter Constraints

#region Derives from specific base type
    /// <summary>
    /// This constraint limits <typeparamref name="T"/> to <see cref="Delegate"/> types.
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/generics/constraints-on-type-parameters#delegate-constraints"/>
    [StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
    public readonly struct IsDelegate<T>
        where T : Delegate;

    /// <summary>
    /// This constraint requires <typeparamref name="T"/> to be an <see cref="Enum"/>.
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/generics/constraints-on-type-parameters#enum-constraints"/>
    [StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
    public readonly struct IsEnum<T>
        where T : Enum;

    /// <summary>
    /// This constraint requires <typeparamref name="T"/> to be a <see langword="struct"/> and <see cref="Enum"/>.
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/generics/constraints-on-type-parameters#enum-constraints"/>
    [StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
    public readonly struct IsStructEnum<T>
        where T : struct, Enum;
#endregion / Derives from specific base type

#region Implements specific interface
    /// <summary>
    /// Type argument <typeparamref name="T"/> must implement the <see cref="IEquatable{T}"/> <see langword="interface"/>.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
    public readonly struct HasIEquatable<T>
        where T : IEquatable<T>;

    /// <summary>
    /// Type argument <typeparamref name="T"/> must implement the <see cref="IEquatable{T}"/> <see langword="interface"/>.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
    public readonly struct HasIComparable<T>
        where T : IComparable;

    /// <summary>
    /// Type argument <typeparamref name="T"/> must implement the <see cref="IEquatable{TOther}"/> <see langword="interface"/>.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
    public readonly struct HasIComparable<T, TOther>
        where T : IComparable<TOther>;

    /// <summary>
    /// Type argument <typeparamref name="T"/> must implement the <see cref="IConvertible"/> <see langword="interface"/>.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
    public readonly struct HasIConvertible<T>
        where T : IConvertible;

    /// <summary>
    /// Type argument <typeparamref name="T"/> must implement the <see cref="IFormattable"/> <see langword="interface"/>.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
    public readonly struct HasIFormattable<T>
        where T : IFormattable;

#if NET6_0_OR_GREATER
    /// <summary>
    /// Type argument <typeparamref name="T"/> must implement the <see cref="ISpanFormattable"/> <see langword="interface"/>.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
    public readonly struct HasISpanFormattable<T>
        where T : ISpanFormattable;
#endif

    /// <summary>
    /// Type argument <typeparamref name="T"/> must implement the <see cref="IDisposable"/> <see langword="interface"/>.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
    public readonly struct HasIDisposable<T>
        where T : IDisposable;

#if NETSTANDARD2_1 || NET6_0_OR_GREATER
    /// <summary>
    /// Type argument <typeparamref name="T"/> must implement the <see cref="IDisposable"/> <see langword="interface"/>.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
    public readonly struct HasIAsyncDisposable<T>
        where T : IAsyncDisposable;
#endif

#if NET7_0_OR_GREATER
    /// <summary>
    /// Type argument <typeparamref name="TSelf"/> must implement the <see cref="IParsable{T}"/> <see langword="interface"/>.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
    public readonly struct HasIParsable<TSelf>
        where TSelf : IParsable<TSelf>;

    /// <summary>
    /// Type argument <typeparamref name="TSelf"/> must implement the <see cref="ISpanParsable{T}"/> <see langword="interface"/>.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
    public readonly struct HasISpanParsable<TSelf>
        where TSelf : ISpanParsable<TSelf>;

    /// <summary>
    /// Type argument <typeparamref name="TSelf"/> must implement the <see cref="INumberBase{T}"/> <see langword="interface"/>.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
    public readonly struct HasINumberBase<TSelf>
        where TSelf : INumberBase<TSelf>;

    /// <summary>
    /// Type argument <typeparamref name="T"/> must implement the <see cref="IEqualityOperators{TSelf,TOther,TResult}"/> <see langword="interface"/>.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
    public readonly struct HasIEqualityOperators<T>
        where T : IEqualityOperators<T, T, bool>;

    /// <summary>
    /// Type argument <typeparamref name="TSelf"/> must implement the <see cref="IEqualityOperators{TSelf,TOther,TResult}"/> <see langword="interface"/>.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
    public readonly struct HasIEqualityOperators<TSelf, TOther, TResult>
        where TSelf : IEqualityOperators<TSelf, TOther, TResult>?;

    /// <summary>
    /// Type argument <typeparamref name="T"/> must implement the <see cref="IComparisonOperators{TSelf,TOther,TResult}"/> <see langword="interface"/>.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
    public readonly struct HasIComparisonOperators<T>
        where T : IComparisonOperators<T, T, int>;

    /// <summary>
    /// Type argument <typeparamref name="TSelf"/> must implement the <see cref="IComparisonOperators{TSelf,TOther,TResult}"/> <see langword="interface"/>.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
    public readonly struct HasIComparisonOperators<TSelf, TOther, TResult>
        where TSelf : IComparisonOperators<TSelf, TOther, TResult>?;
#endif
#endregion / Implements specific interface

#region Combinations
    /// <summary>
    /// Type argument <typeparamref name="T"/> must be a reference type with a public parameterless constructor.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
    public readonly struct IsClassHasNew<T>
        where T : class, new();

#region : ..., IDisposable
    /// <summary>
    /// Type argument <typeparamref name="T"/> must be a value type that implements <see cref="IDisposable"/>.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
    public readonly struct IsStructHasIDisposable<T>
        where T : struct, IDisposable;

    /// <summary>
    /// Type argument <typeparamref name="T"/> must be a reference type that implements <see cref="IDisposable"/>.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
    public readonly struct IsClassHasIDisposable<T>
        where T : class, IDisposable;

    /// <summary>
    /// Type argument <typeparamref name="T"/> must implement <see cref="IDisposable"/> and have a public parameterless constructor.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
    public readonly struct HasIDisposableHasNew<T>
        where T : IDisposable, new();
#endregion

#if NET9_0_OR_GREATER
#region : ..., allows ref struct
    /// <summary>
    /// Type argument <typeparamref name="T"/> must be a value type that may also be a <see langword="ref struct"/>.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
    public readonly struct IsStructAllowsRefStruct<T>
        where T : struct, allows ref struct;

    /// <summary>
    /// Type argument <typeparamref name="T"/> must be an <see langword="unmanaged"/> value type that may also be a <see langword="ref struct"/>.
    /// </summary>
    [StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
    public readonly struct IsUnmanagedAllowsRefStruct<T>
        where T : unmanaged, allows ref struct;
#endregion
#endif
#endregion / Combinations
}