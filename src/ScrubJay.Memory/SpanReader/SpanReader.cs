using ScrubJay.Polyfills.Text;
using ScrubJay.Reflection.Lightweight;

namespace ScrubJay.Memory;

[PublicAPI]
[DebuggerDisplay("{Display,nq}")]
[StructLayout(LayoutKind.Auto)]
public ref partial struct SpanReader<T>
{
    internal readonly ReadOnlySpan<T> _span;
    internal readonly int _spanLength;
    internal int _position;

    public int Position
    {
        readonly get => _position;
        set => TrySetPosition(value);
    }

    public readonly bool IsCompleted => _position >= _spanLength;

    public readonly int RemainingLength => _spanLength - _position;

    public readonly ReadOnlySpan<T> Previous => _span[.._position];

    public readonly ReadOnlySpan<T> Next => _span[_position..];

    public SpanReader(ReadOnlySpan<T> span)
    {
        _span = span;
        _spanLength = span.Length;
        _position = 0;
    }

    internal void Deconstruct(out ReadOnlySpan<T> span, out int spanLength, out int position)
    {
        span = _span;
        spanLength = _spanLength;
        position = _position;
    }

    private readonly InvalidOperationException GetMoveException(
        string methodName,
        int count)
    {
        using InterpolatedText message = $"SpanReader<{TypeName.For<T>()}> cannot {methodName} {count} items: ";
        int remains = RemainingLength;
        if (remains == 0)
        {
            message.Write("no items left to read");
        }
        else
        {
            message.Write($"only {remains} items left to read");
        }
        return new InvalidOperationException(message.ToString());
    }

    public bool TrySetPosition(int index)
    {
        if ((uint)index <= (uint)_spanLength)
        {
            _position = index;
            return true;
        }
        return false;
    }

    public bool TrySetPosition(Index index)
    {
        int offset = index.GetOffset(_spanLength);
        if ((uint)offset <= (uint)_spanLength)
        {
            _position = offset;
            return true;
        }
        return false;
    }

    public bool TrySetPosition(SeekOrigin index)
    {
        return index switch
        {
            SeekOrigin.Begin => TrySetPosition(0),
            SeekOrigin.Current => true,
            SeekOrigin.End => TrySetPosition(_spanLength - 1),
            _ => false,
        };
    }

    /// <summary>
    /// Resets this <see cref="SpanReader{T}"/> back to its beginning position
    /// </summary>
    public void Reset() => _position = 0;

    private readonly string Display()
    {
        using var builder = new InterpolatedText(24, 8);

        var pos = _position;
        // how many items before/after do we want to show?
        int previewCount;

        // chars we show more
        if (typeof(T) == typeof(char))
        {
            previewCount = 16;
        }
        else
        {
            previewCount = 4;
        }

        // previous items
        var previousStart = pos - previewCount;
        if (previousStart <= 0)
        {
            // hard clamp
            previousStart = 0;
        }
        else
        {
            // there are previous items we are not showing
            builder.Write("… ");
        }
        builder.Write(_span.Slice(previousStart, pos - previousStart));

        // we are here
        builder.Write(" ⌖ ");

        // next items
        var nextEnd = pos + previewCount;
        if (nextEnd >= _spanLength)
        {
            // all the rest
            builder.Write(_span[_position..]);
        }
        else
        {
            builder.Write(_span.Slice(pos, nextEnd - pos));
            // there are more next items we are not showing
            builder.Write(" …");
        }

        return builder.ToString();
    }

    public readonly override string ToString() => $"[0.. |{_position}| ..{_spanLength})";
}