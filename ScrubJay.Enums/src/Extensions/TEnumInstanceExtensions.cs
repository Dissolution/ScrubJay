namespace ScrubJay.Enums.Extensions;

/// <summary>
/// Extensions on <c>TEnum</c> instances <c>where TEnum : struct, Enum</c>
/// </summary>
[PublicAPI]
public static class TEnumInstanceExtensions
{
    // readonly extensions
    extension<E>(E @enum)
        where E : struct, Enum
    {
        public string ToString(EnumPart part, string? format = null)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(E other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(E other)
        {
            throw new NotImplementedException();
        }
    }
}

public static class TFlaggedEnumInstanceExtensions
{
    // readonly extensions
    extension<E>(E @enum)
        where E : struct, Enum
    {
        public bool HasFlag(E flag)
        {
            throw new NotImplementedException();
        }

        public EnumEnumerable<E> AsEnumerable()
        {
            return new(@enum);
        }

        public EnumEnumerator<E> GetEnumerator()
        {
            return new(@enum);
        }

        public bool All(E flag)
        {
            throw new NotImplementedException();
        }

        public bool All(params ReadOnlySpan<E> flags)
        {
            throw new NotImplementedException();
        }

        public bool All(IEnumerable<E>? flags)
        {
            throw new NotImplementedException();
        }
        
        public bool Any(E flag)
        {
            throw new NotImplementedException();
        }

        public bool Any(params ReadOnlySpan<E> flags)
        {
            throw new NotImplementedException();
        }

        public bool Any(IEnumerable<E>? flags)
        {
            throw new NotImplementedException();
        }
    }
}

public struct EnumEnumerable<E> : IEnumerable<E>
    where E : struct, Enum
{
    public EnumEnumerable(E @enum)
    {
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    IEnumerator<E> IEnumerable<E>.GetEnumerator() => GetEnumerator();

    public EnumEnumerator<E> GetEnumerator()
    {
        throw new NotImplementedException();
    }
}

public struct EnumEnumerator<E> : IEnumerator<E> 
    where E : struct, Enum
{
    object IEnumerator.Current => Current;
    
    public E Current { get; }
    
    public EnumEnumerator(E @enum)
    {
    }

    public bool MoveNext()
    {
        throw new NotImplementedException();
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}


/// <summary>
/// Specifies a particular part of an <see langword="enum"/> member.
/// </summary>
[PublicAPI]
public enum EnumPart
{
    /// <summary>
    /// The name of the member (as it was declared)
    /// </summary>
    Name,
    
    /// <summary>
    /// The underlying value of the member
    /// </summary>
    Value,
    
    /// <summary>
    /// A defined <see cref="Attribute"/> format
    /// </summary>
    Attribute,
}