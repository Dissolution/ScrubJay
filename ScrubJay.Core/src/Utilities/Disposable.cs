#pragma warning disable CA1031, MA0048, CA1045

namespace ScrubJay.Utilities;

#if !(NETSTANDARD2_0 || NETFRAMEWORK)
/// <summary>
/// Helper utility for working with <see cref="IDisposable"/> and <see cref="IAsyncDisposable"/>
/// </summary>
#else
/// <summary>
/// Helper utility for working with <see cref="IDisposable"/>
/// </summary>
#endif
[PublicAPI]
public static class Disposable
{
    /// <summary>
    /// Gets an <see cref="IDisposable"/> that will invoke an <see cref="Action"/> when it is disposed
    /// </summary>
    /// <param name="onDispose">
    /// The <see cref="Action"/> that will be invoked during disposal
    /// </param>
    [MustDisposeResource]
    public static IDisposable Action(Action onDispose)
    {
        Throw.IfNull(onDispose);
        return new ActionDisposable(onDispose);
    }

#if !(NETSTANDARD2_0 || NETFRAMEWORK)
    /// <summary>
    /// Gets an <see cref="IDisposable"/> that will asynchronously invoke an <see cref="Action"/> when it is disposed
    /// </summary>
    /// <param name="onDispose">
    /// The <c>async</c> <see cref="Action"/> that will be invoked during disposal
    /// </param>
    [MustDisposeResource]
    public static IAsyncDisposable AsyncAction(Func<ValueTask> onDispose)
    {
        Throw.IfNull(onDispose);
        return new ActionAsyncDisposable(onDispose);
    }
#endif

    public static void Dispose<T>([HandlesResourceDisposal] T? value)
    {
        if (value is IDisposable)
        {
            ((IDisposable)value).Dispose();
        }
    }

    public static void Dispose([HandlesResourceDisposal] IDisposable? disposable)
        => disposable?.Dispose();
}

internal sealed class ActionDisposable : IDisposable
{
    private readonly Action _onDispose;

    internal ActionDisposable(Action onDispose)
    {
        _onDispose = onDispose;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose() => _onDispose();
}

#if !(NETSTANDARD2_0 || NETFRAMEWORK)
internal sealed class ActionAsyncDisposable : IAsyncDisposable
{
    private readonly Func<ValueTask> _onAsyncDispose;

    public ActionAsyncDisposable(Func<ValueTask> onAsyncDispose)
    {
        _onAsyncDispose = onAsyncDispose;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueTask DisposeAsync() => _onAsyncDispose(); // no await, direct pass-through
}
#endif