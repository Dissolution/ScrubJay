using ScrubJay.Errors.Validation;
using ScrubJay.Polyfills.Text;
using ScrubJay.Reflection.Lightweight;

namespace ScrubJay.Memory;

[PublicAPI]
[DebuggerDisplay("{Display,nq}")]
[StructLayout(LayoutKind.Auto)]
public ref partial struct SpanWriter<T>
{
    internal Span<T> _span;
    internal readonly int _spanLength;
    internal int _position;

    public int Position
    {
        readonly get => _position;
        set => TrySetPosition(value);
    }

    public readonly int Capacity => _spanLength;

    public readonly int RemainingCapacity => _spanLength - _position;

    public readonly Span<T> Written => _span[.._position];

    public readonly Span<T> Available => _span[_position..];
    
    public readonly ref T this[Index index] => ref _span[.._position][index];

    public Span<T> this[Range range] => _span[.._position][range];

    public SpanWriter(Span<T> span)
    {
        _span = span;
        _spanLength = span.Length;
        _position = 0;
    }
    
    public SpanWriter(Span<T> span, Index position)
    {
        _span = span;
        _spanLength = span.Length;
        _position = Demand.InRange(position.GetOffset(_spanLength), 0, _spanLength+1);
    }

    internal void Deconstruct(out Span<T> span, out int spanLength, out int position)
    {
        span = _span;
        spanLength = _spanLength;
        position = _position;
    }
    
    private readonly InvalidOperationException GetWriteException(
        string methodName,
        int count)
    {
        using InterpolatedText message = $"SpanWriter<{TypeName.For<T>()}> cannot {methodName} {count} items: ";
        int remains = RemainingCapacity;
        if (remains == 0)
        {
            message.Write("no capacity remains");
        }
        else
        {
            message.Write($"only {remains} capacity remains");
        }
        return new InvalidOperationException(message.ToString());
    }


    public bool TryUseAvailable([NotNullWhen(true)] SpanFunc<T, int>? useAvailable)
    {
        if (useAvailable is not null)
        {
            int used = useAvailable(Available);
            if (used >= 0 && used <= RemainingCapacity)
            {
                _position += used;
                return true;
            }
        }
        
        return false;
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

    public void Reset()
    {
        _span.Slice(0, _position).Clear();
        _position = 0;
    }
    
    private readonly string Display()
    {
        using var builder = new InterpolatedText(24, 8);

        var pos = _position;
        // how many written items do we want to show?
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

        return builder.ToString();
    }

    public readonly override string ToString() => $"[0.. |{_position}| ..{_spanLength})";
}