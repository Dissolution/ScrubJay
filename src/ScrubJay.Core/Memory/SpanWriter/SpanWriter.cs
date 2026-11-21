using ScrubJay.Errors;
using ScrubJay.Text;

namespace ScrubJay.Memory.SpanWriter;

[PublicAPI]
[DebuggerDisplay("{Display,nq}")]
[StructLayout(LayoutKind.Auto)]
public ref partial struct SpanWriter<T>
{
    internal Span<T> _span;
    internal int _position;

    public int Position
    {
        readonly get => _position;
        set => TrySetPosition(value);
    }

    public readonly int Capacity => _span.Length;

    public readonly int AvailableCapacity => _span.Length - _position;

    public readonly Span<T> Written => _span[.._position];

    public readonly Span<T> Available => _span[_position..];

    public ref T this[int index] => ref _span[.._position][index];

    public ref T this[Index index] => ref _span[.._position][index];

    public Span<T> this[Range range] => _span[.._position][range];

    public SpanWriter(Span<T> span)
    {
        _span = span;
        _position = 0;
    }

    public SpanWriter(Span<T> span, int position)
    {
        _span = span;
        _position = Ensure.Index(position, _span.Length);
    }

    public SpanWriter(Span<T> span, Index position)
    {
        _span = span;
        _position = Ensure.Index(position, _span.Length);
    }

    internal void Deconstruct(out Span<T> span, out int spanLength, out int position)
    {
        span = _span;
        spanLength = _span.Length;
        position = _position;
    }

    private readonly InvalidOperationException GetWriteException(
        string methodName,
        int count)
    {
        Debug.Assert(count > 0);

        using Interpolator message = $"SpanWriter<{typeof(T).Alias}> cannot {methodName} {count} item";
        
        if (count > 1)
        {
            message.Write('s');
        }

        int available = AvailableCapacity;
        if (available == 0)
        {
            message.Write(": no capacity remains.");
        }
        else
        {
            message.Write($": only {available} capacity remains.");
        }

        return new InvalidOperationException(message);
    }


    public bool TryUseAvailable([NotNullWhen(true)] Func<Span<T>, int>? useAvailable)
    {
        if (useAvailable is not null)
        {
            int used = useAvailable(Available);
            if (used >= 0 && used <= AvailableCapacity)
            {
                _position += used;
                return true;
            }
        }

        return false;
    }


    public bool TrySetPosition(int index)
    {
        if ((uint)index <= (uint)_span.Length)
        {
            _position = index;
            return true;
        }
        return false;
    }

    public bool TrySetPosition(Index index)
    {
        int offset = index.GetOffset(_span.Length);
        if ((uint)offset <= (uint)_span.Length)
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
            SeekOrigin.End => TrySetPosition(_span.Length - 1),
            _ => false,
        };
    }

    public void Reset()
    {
        _span.Slice(0, _position).Clear();
        _position = 0;
    }

    private readonly string Display()
    {
//        using var builder = new Interpolator();
//
//        var pos = _position;
//        // how many written items do we want to show?
//        int previewCount;
//
//        // chars we show more
//        if (typeof(T) == typeof(char))
//        {
//            previewCount = 16;
//        }
//        else
//        {
//            previewCount = 4;
//        }
//
//        // previous items
//        var previousStart = pos - previewCount;
//        if (previousStart <= 0)
//        {
//            // hard clamp
//            previousStart = 0;
//        }
//        else
//        {
//            // there are previous items we are not showing
//            builder.Write("… ");
//        }
//
//        builder.Write(_span.Slice(previousStart, pos - previousStart));
//
//        return builder.ToString();
        return ToString();
    }

    public override readonly string ToString() => $"[0, …, {_position}, …, {_span.Length})";
}