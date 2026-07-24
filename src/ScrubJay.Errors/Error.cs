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

public class Error : IEqualityOperators<Error, Error, bool>, IEquatable<Error>
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

    
    
    


    public virtual bool Equals(Error? other)
    {
        
    }
}