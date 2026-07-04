// ReSharper disable RedundantBaseQualifier

namespace ScrubJay.Polyfills.Collections;

/// <summary>
/// A <see cref="HashSet{T}"/> that supports generic-typed operations.
/// </summary>
[PublicAPI]
public class TypeSet : HashSet<Type>
{
    public bool Add<T>() => base.Add(typeof(T));

    public bool Contains<T>() => base.Contains(typeof(T));
}

public static class TypeCollectionExtensions
{
    extension(IEnumerable<Type>? types)
    {
        public TypeSet ToTypeSet()
        {
            var typeset = new TypeSet();
            typeset.AddMany(types);
            return typeset;
        }
    }
}