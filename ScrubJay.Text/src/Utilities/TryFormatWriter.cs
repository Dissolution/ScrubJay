namespace ScrubJay.Text.Utilities;

public ref struct TryFormatWriter : IEnumerable<char>, IEnumerable
{
    private Span<char> _destination;
    private int _written; // if negative, writing has failed

    public TryFormatWriter(Span<char> destination)
    {
        _destination = destination;
        _written = 0;
    }

    public void Add(char ch)
    {
        int pos = _written;
        var dest = _destination;

        if (pos < 0) return;
        if (pos >= dest.Length)
        {
            _written = -1;
            return;
        }

        dest[pos] = ch;
        _written = pos + 1;
    }

    public void Add(string? str)
    {
        if (str is null || _written < 0) return;
        if (str.TryCopyTo(_destination[_written..]))
        {
            _written += str.Length;
        }
        else
        {
            _written = -1;
        }
    }

    public void Add<T>(in T? value)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
        => Add(Any.ToString<T>(in value));

    public void Add<T>(in T? value, string? format)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        if (Any.HasTryFormat<T>())
        {
            if (Any.TryFormat<T>(in value, _destination, out var cw, format))
            {
                _written += cw;
            }
            else
            {
                _written = -1;
            }
        }
        else
        {
            Add(Any.Format<T>(in value, format));
        }
    }

    public void Add<T>(in T? value, scoped text format)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        if (Any.HasTryFormat<T>())
        {
            if (Any.TryFormat<T>(in value, _destination, out var cw, format))
            {
                _written += cw;
            }
            else
            {
                _written = -1;
            }
        }
        else
        {
            Add(Any.Format<T>(in value, format.ToString()));
        }
    }

    public void Add<T>(in T? value, string? format, IFormatProvider? provider)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        if (Any.HasTryFormat<T>())
        {
            if (Any.TryFormat<T>(in value, _destination, out var cw, format, provider))
            {
                _written += cw;
            }
            else
            {
                _written = -1;
            }
        }
        else
        {
            Add(Any.Format<T>(in value, format, provider));
        }
    }

    public void Add<T>(in T? value, scoped text format, IFormatProvider? provider)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        if (Any.HasTryFormat<T>())
        {
            if (Any.TryFormat<T>(in value, _destination, out var cw, format, provider))
            {
                _written += cw;
            }
            else
            {
                _written = -1;
            }
        }
        else
        {
            Add(Any.Format<T>(in value, format.ToString(), provider));
        }
    }

    public bool Wrote(out int charsWritten)
    {
        charsWritten = _written;
        return charsWritten >= 0;
    }
    
    IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();

    IEnumerator<char> IEnumerable<char>.GetEnumerator() => throw new NotImplementedException();

    public Span<char>.Enumerator GetEnumerator() => _destination.Slice(0, _written).GetEnumerator();
}