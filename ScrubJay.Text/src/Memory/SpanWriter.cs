namespace ScrubJay.Text.Memory;

[PublicAPI]
public ref struct SpanWriter<T>
{
    private Span<T> _span;
    private int _position;

    public SpanWriter(Span<T> span)
    {
        _span = span;
        _position = 0;
    }
}