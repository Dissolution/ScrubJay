// Exception to Identifiers Require Correct Suffix

using ScrubJay.Collections.Iteration;
#pragma warning disable CA1710
// Only implements IEnumerable for fluent collection initialization support
#pragma warning disable CA1010

// ReSharper disable MergeCastWithTypeCheck

namespace ScrubJay.Text;

/// <summary>
///
/// </summary>
/// <remarks>
/// Supports <a href="https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/object-and-collection-initializers#collection-initializers">
/// fluent collection initialization</a>
/// </remarks>
[PublicAPI]
[StructLayout(LayoutKind.Auto)]
public ref struct TryFormatter : IEnumerable
{
    private Span<char> _destination;
    private int _position;
    private bool _hasFailed;

    private Span<char> Available
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _destination.Slice(_position);
    }

    private readonly int End
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _destination.Length;
    }

    public TryFormatter(Span<char> destination)
    {
        _destination = destination;
        _position = 0;
        _hasFailed = false;
    }

    public TryFormatter(Span<char> destination, int position)
    {
        if ((position < 0) || (position >= destination.Length))
            throw new ArgumentOutOfRangeException(nameof(position), position, "Position must be inside of span");
        _destination = destination;
        _position = position;
        _hasFailed = false;
    }

    private bool SetError()
    {
        _hasFailed = true;
        Clear();
        return false;
    }

    public bool Add(char ch)
    {
        if (_hasFailed)
            return false;

        int pos = _position;
        int newPos = pos + 1;
        if (newPos <= End)
        {
            _destination[pos] = ch;
            _position = newPos;
            return true;
        }

        return SetError();
    }


    public bool Add(scoped text text)
    {
        if (_hasFailed)
            return false;

        if (text.TryCopyTo(Available))
        {
            _position += text.Length;
            return true;
        }

        return SetError();
    }

    public bool Add(string? str) => Add(str.AsSpan());

    public bool Add<T>(T? value)
    {
        if (_hasFailed)
            return false;

        string? str;
        if (value is IFormattable)
        {
            // If the value can format itself directly into our buffer, do so
            if (value is ISpanFormattable)
            {
                if (!((ISpanFormattable)value).TryFormat(Available, out int charsWritten, default, default))
                {
                    return SetError();
                }

                _position += charsWritten;
                return true;
            }

            str = ((IFormattable)value).ToString(null, null);
        }
        else
        {
            str = value?.ToString();
        }

        return Add(str);
    }

    public bool Add<T>(T? value,
        string? format,
        IFormatProvider? provider = null)
    {
        if (_hasFailed)
            return false;

        string? str;
        if (value is IFormattable)
        {
            // If the value can format itself directly into our buffer, do so
            if (value is ISpanFormattable)
            {
                if (!((ISpanFormattable)value).TryFormat(Available, out int charsWritten, format.AsSpan(), provider))
                {
                    return SetError();
                }

                _position += charsWritten;
                return true;
            }

            str = ((IFormattable)value).ToString(format, provider);
        }
        else
        {
            str = value?.ToString();
        }

        return Add(str);
    }

    public bool Add<T>(T? value,
        text format,
        IFormatProvider? provider = null)
    {
        if (_hasFailed)
            return false;

        string? str;
        if (value is IFormattable)
        {
            // If the value can format itself directly into our buffer, do so
            if (value is ISpanFormattable)
            {
                if (!((ISpanFormattable)value).TryFormat(Available, out int charsWritten, format, provider))
                {
                    return SetError();
                }

                _position += charsWritten;
                return true;
            }

            str = ((IFormattable)value).ToString(format.AsString(), provider);
        }
        else
        {
            str = value?.ToString();
        }

        return Add(str);
    }

    public bool Add<T>((T? Value, string? Format) valueTuple)
        => Add<T>(valueTuple.Value, valueTuple.Format);

    public bool Add<T>(Tuple<T?, string?> tuple)
        => Add<T>(tuple.Item1, tuple.Item2);

    public bool Add<T>((T? Value, string? Format, IFormatProvider? Provider) valueTuple)
        => Add<T>(valueTuple.Value, valueTuple.Format, valueTuple.Provider);

    public bool Add<T>(Tuple<T?, string?, IFormatProvider?> tuple)
        => Add<T>(tuple.Item1, tuple.Item2, tuple.Item3);


    public delegate bool FormatWriterAdd(ref TryFormatter writer);

    public bool Add(FormatWriterAdd del) => del(ref this);

    public void Clear()
    {
        _destination.Clear();
        _position = 0;
    }

    public bool TryCopyTo(Span<char> span) => _destination.Slice(0, _position).TryCopyTo(span);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<char> AsSpan() => _destination.Slice(0, _position);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public char[] ToArray() => _destination.Slice(0, _position).ToArray();
    
    public readonly bool GetResult(out int charsWritten)
    {
        if (!_hasFailed)
        {
            charsWritten = _position;
            return true;
        }

        // remove any trace of us
        _destination.Clear();
        charsWritten = 0;
        return false;
    }

    public readonly string GetString()
    {
        if (_hasFailed)
            return string.Empty;
        return _destination[.._position].AsString();
    }

    public override string ToString() => _destination.Slice(0, _position).AsString();

    readonly IEnumerator IEnumerable.GetEnumerator()
    {
        return Iterate.Empty();
    }
}