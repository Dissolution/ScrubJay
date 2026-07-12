namespace ScrubJay.Debugging;

[PublicAPI]
internal sealed class ActionDisposable : IDisposable
{
    private Action? _onDispose;

    public ActionDisposable(Action? onDispose)
    {
        _onDispose = onDispose;
    }
    ~ActionDisposable()
    {
        this.Dispose();
    }

    public void Dispose()
    {
        var onDispose = Interlocked.Exchange(ref _onDispose, null);

        try
        {
            onDispose?.Invoke();
        }
        catch
        {
            // swallow everything
        }

        GC.SuppressFinalize(this);
    }
}