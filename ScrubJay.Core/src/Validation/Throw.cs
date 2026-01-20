#pragma warning disable CA1716

namespace ScrubJay.Validation;

public static partial class Throw
{
    public static void IfNull<T>([AllowNull, NotNull, NoEnumeration] T value, [CallerArgumentExpression(nameof(value))] string? valueName = null)
    {
        if (value is not null)
            return;
        throw Ex.ArgNull(value, valueName);
    }
    
    public static void IfBadEnumeration(int currentVersion, int oldVersion)
    {
        if (currentVersion != oldVersion)
        {
            throw new InvalidOperationException("Cannot continue enumerating -- version has changed");
        }
    }

    public static void IfDisposed<T>(
        T instance, 
        [DoesNotReturnIf(true)] bool disposed,
        [CallerArgumentExpression(nameof(instance))] string? instanceName = null)
    {
        if (disposed)
            throw new ObjectDisposedException(instanceName, $"{TypeName.For<T>()} instance was disposed");
    }
}