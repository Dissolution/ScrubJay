namespace ScrubJay.Debugging.Destinations.Console;

public struct ConsoleColorReset : IDisposable
{
    private readonly ConsoleColorChange _change;

    public ConsoleColorReset(ConsoleColorChange change)
    {
        _change = change;
    }

    public void Dispose()
    {
        if (_change.ChangesForeground(out var newFore))
        {
            System.Console.ForegroundColor = newFore;
        }
        if (_change.ChangesBackground(out var newBack))
        {
            System.Console.BackgroundColor = newBack;
        }
    }
}