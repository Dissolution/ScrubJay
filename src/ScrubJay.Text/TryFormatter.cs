using ScrubJay.Polyfills.Collections;
using ScrubJay.Universal;

namespace ScrubJay.Text;

[PublicAPI]
[StructLayout(LayoutKind.Auto)]
public ref struct TryFormatter : IEnumerable<char>, IEnumerable
{
    private SpanWriter<char> _writer;
    private bool _failed;

    public readonly bool Failed => _failed;
    
    public TryFormatter(Span<char> destination)
    {
        _writer = new SpanWriter<char>(destination);
        _failed = false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void SetFailed()
    {
        _failed = true;
        _writer.Reset();
    }
    
    public void Add(char ch)
    {
        if (!_failed && !_writer.TryWrite(ch))
            SetFailed();
    }

    public void Add(string? str)
    {
        if (!_failed && !_writer.TryWrite(str))
            SetFailed();
    }
    
    public void Add(scoped text text)
    {
        if (!_failed && !_writer.TryWrite(text))
            SetFailed();
    }

    public void Add<T>(T? value)
    {
        if (!_failed && !_writer.TryWrite<T>(value))
            SetFailed();
    }

#if NET9_0_OR_GREATER
    public void Add<T>(in T? value, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        if (!_failed && !_writer.TryWrite<T>(value, _))
            SetFailed();
    }
#endif

    public void Add<T>(T? value, string? format, IFormatProvider? provider = null)
    {
        if (!_failed && !_writer.TryWrite<T>(value, format, provider))
            SetFailed();
    }

    public void Add<T>(T? value, text format, IFormatProvider? provider)
    {
        if (!_failed && !_writer.TryWrite<T>(value, format, provider))
            SetFailed();
    }

   
    public readonly bool Wrote(out int charsWritten)
    {
        if (_failed)
        {
            charsWritten = 0;
            return false;
        }
        else
        {
            charsWritten = _writer.Position;
            return true;
        }
    }

    readonly IEnumerator IEnumerable.GetEnumerator() => Enumeration.Empty();

    readonly IEnumerator<char> IEnumerable<char>.GetEnumerator() => Enumeration.Empty<char>();
}