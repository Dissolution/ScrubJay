namespace ScrubJay.Errors;

public enum ErrorKind
{
    Failure, // 400
    Validation, // 400
    Unauthorized, // 401
    Forbidden, // 403
    NotFound, // 404
    Conflict, // 409
    Unexpected, // 500
}

// not a record so that collections can be properties

public class Error :
#if NET7_0_OR_GREATER
    IEqualityOperators<Error, Error, bool>,
#endif
    IEquatable<Error>
{
    public static bool operator ==(Error? left, Error? right)
    {
        if (left is not null)
            return left.Equals(right);
        if (right is not null)
            return right.Equals(left);
        return true;
    }

    public static bool operator !=(Error? left, Error? right)
    {
        if (left is not null)
            return !left.Equals(right);
        if (right is not null)
            return !right.Equals(left);
        return false;
    }


    public required ErrorKind Kind { get; init; }
    public required string Title { get; init; }
    public required string Details { get; init; }
    public Exception? Exception { get; init; } = null;
    public IReadOnlyDictionary<string, object?> Data { get; init; } = new Dictionary<string, object?>(capacity: 0);
    
    public virtual bool Equals(Error? other)
    {
        if (other is null)
            return false;
        return Relate.Equate(Kind, other.Kind) &&
            Relate.Equate(Title, other.Title) &&
            Relate.Equate(Details, other.Details) &&
            Relate.NullEquate(Exception, other.Exception,
                static (left, right) => Relate.Equate(left.GetType(), right.GetType()) && Relate.Equate(left.Message, right.Message)) &&
            Relate.Equate(Data, other.Data);
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is Error error)
            return Equals(error);
        return false;
    }

    public override int GetHashCode()
    {
        HashCode hasher = new();
        hasher.Add(Kind);
        hasher.Add(Title, StringComparer.Ordinal);
        hasher.Add(Details, StringComparer.Ordinal);
        if (Exception is not null)
        {
            hasher.Add(Exception.GetType());
            hasher.Add(Exception.Message);
        }
        foreach (var pair in Data)
        {
            hasher.Add(pair.Key);
            hasher.Add(pair.Value);
        }
        return hasher.ToHashCode();
    }

    public override string ToString()
    {
        
    }
}

internal static class Relate
{
    public static bool Equate<T>(T? left, T? right, TypeConstraints.HasIEquatable<T> _ = default)
        where T : IEquatable<T>
    {
        if (left is not null)
        {
            return left.Equals(right);
        }
        else if (right is not null)
        {
            return right.Equals(left);
        }
        else
        {
            return true;
        }
    }
    
    public static bool Equate<E>(E left, E right, TypeConstraints.IsStructEnum<E> _ = default)
        where E : struct, Enum
    {
        Emit.Ldarg(nameof(left));
        Emit.Ldarg(nameof(right));
        Emit.Ceq();
        return Return<bool>();
    }

    public static bool Equate(scoped text left, scoped text right, StringComparison comparison = StringComparison.Ordinal)
    {
        return MemoryExtensions.Equals(left, right, comparison);
    }

    public static bool Equate(Type? left, Type? right) => left == right;

    public static bool Equate<K, V>(
        IReadOnlyDictionary<K, V>? left,
        IReadOnlyDictionary<K, V>? right,
        IEqualityComparer<object?>? valueComparer = null)
    => NullEquate(left, right, (l, r) =>
    {
        if (l.Count != r.Count)
            return false;
        valueComparer ??= EqualityComparer<object?>.Default;
        foreach (var pair in l)
        {
            if (!r.TryGetValue(pair.Key, out var rValue))
                return false;
            if (!valueComparer.Equals(pair.Value, rValue))
                return false;
        }
        return true;
    })

    public static bool NullEquate<T>(T? left, T? right, Func<T, T, bool> nonNullEquate)
    {
        if (left is not null)
        {
            if (right is not null)
            {
                return nonNullEquate(left, right);
            }
            else
            {
                return false;
            }
        }
        else
        {
            return right is null;
        }
    }
}