namespace ScrubJay.Text.Building;

public sealed class DisposeTextBuild : IDisposable
{
    private readonly TextBuilder _textBuilder;
    private Action<TextBuilder>? _build;

    public DisposeTextBuild(TextBuilder textBuilder, Action<TextBuilder>? build)
    {
        _textBuilder = textBuilder;
        _build = build;
    }

    public void Dispose()
    {
        if (_build is not null)
        {
            _build(_textBuilder);
            _build = null;
        }
    }
}