// ReSharper disable RedundantBaseQualifier

namespace ScrubJay.Collections;

/// <summary>
/// A <see cref="HashSet{T}"/> that supports generic-typed operations.
/// </summary>
[PublicAPI]
public class TypeSet : HashSet<Type>
{
    public bool Add<T>() => base.Add(typeof(T));

    public bool Contains<T>() => base.Contains(typeof(T));
}