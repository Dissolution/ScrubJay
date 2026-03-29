namespace ScrubJay.Enhancements.Exceptions;

[PublicAPI]
public static partial class Ex
{
#region NotImplementedException
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static NotImplementedException NotImplemented()
    {
        return new NotImplementedException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static NotImplementedException NotImplemented(string? message)
    {
        return new NotImplementedException(message);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static NotImplementedException NotImplemented(string? message, Exception? innerException)
    {
        return new NotImplementedException(message, innerException);
    }
#endregion

#region UnreachableException
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static UnreachableException Unreachable()
    {
        return new UnreachableException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static UnreachableException Unreachable(string? message)
    {
        return new UnreachableException(message);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static UnreachableException Unreachable(string? message, Exception? innerException)
    {
        return new UnreachableException(message, innerException);
    }
#endregion

#region InvalidOperationException
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static InvalidOperationException Invalid()
    {
        return new InvalidOperationException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static InvalidOperationException Invalid(string? message)
    {
        return new InvalidOperationException(message);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static InvalidOperationException Invalid(string? message, Exception? innerException)
    {
        return new InvalidOperationException(message, innerException);
    }
#endregion
}