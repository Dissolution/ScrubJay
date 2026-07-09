using ScrubJay.Polyfills.Text;

namespace ScrubJay.Text;

[InterpolatedStringHandler]
[MustDisposeResource(true)]
public ref struct InterpolatedText : IDisposable
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int GetMinCapacity(int literalLength, int formattedCount)
    {
        return literalLength + (formattedCount * 16);
    }

    private char[]? _charArray;
    private Span<char> _chars;
    private int _position;

    public Span<char> Written
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _chars.Slice(0, _position);
    }

    internal Span<char> Available
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _chars.Slice(_position);
    }

    public readonly int Capacity
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _chars.Length;
    }

    public int Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly get => _position;
        internal set => _position = value;
    }

    [MustDisposeResource(false)]
    public InterpolatedText(Span<char> initialBuffer)
    {
        _charArray = null;
        _chars = initialBuffer;
        _position = 0;
    }

    [MustDisposeResource(true)]
    public InterpolatedText(int literalLength, int formattedCount)
    {
        _chars = _charArray = TextPool.Rent(GetMinCapacity(literalLength, formattedCount));
        _position = 0;
    }

    [MustDisposeResource(false)]
    public InterpolatedText(int literalLength, int formattedCount, Span<char> initialBuffer)
    {
        _charArray = null;
        _chars = initialBuffer;
        _position = 0;
    }

    [MustDisposeResource(false)]
    public InterpolatedText(int literalLength, int formattedCount, InterpolatedText interpolator)
    {
        _chars = interpolator._chars;
        _charArray = interpolator._charArray;
        _position = interpolator._position;
    }

    private void GrowCore(int minCapacity)
    {
        Debug.Assert(minCapacity > Capacity);
        char[] newArray = TextPool.Rent(minCapacity);
        if (_position > 0)
        {
            TextHelper.Unsafe.CopyCharacters(_chars, newArray, _position);
        }
        TextPool.Return(_charArray);
        _chars = _charArray = newArray;
    }


    public void GrowBy(int count)
    {
        if (count <= 0) return;
        int newCapacity = _chars.Length + count;
        GrowTo(newCapacity);
    }

    public void GrowTo(int minCapacity)
    {
        if (minCapacity <= _chars.Length) return;
        GrowCore(minCapacity);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void GrowThenAppend(scoped text text)
    {
        int newPos = _position + text.Length;
        Debug.Assert(newPos >= Capacity);
        GrowCore(newPos);
        TextHelper.Unsafe.CopyCharacters(text, Available, text.Length);
        _position += text.Length;
    }

    public void AppendLiteral(string str)
    {
        Debug.Assert(str is not null);
        if (str.TryCopyTo(Available))
        {
            _position += str!.Length;
            return;
        }
        GrowThenAppend(str);
    }

    public void AppendFormatted(
        [HandlesResourceDisposal] [InterpolatedStringHandlerArgument("")]
        ref InterpolatedText interpolated)
    {
        // take back what it took
        _charArray = interpolated._charArray;
        _chars = interpolated._chars;
        _position = interpolated._position;
    }

    public void AppendFormatted<T>(in T? value)
    {
        string? str;
        if (value is IFormattable)
        {
#if NET6_0_OR_GREATER
            if (value is ISpanFormattable)
            {
                int charsWritten;
                while (!((ISpanFormattable)value).TryFormat(Available, out charsWritten, default, default))
                {
                    GrowBy(1);
                }
                _position += charsWritten;
                return;
            }
#endif
            str = ((IFormattable)value).ToString(default, default);
        }
        else
        {
            str = value?.ToString();
        }

        if (str is not null)
        {
            AppendLiteral(str);
        }
    }

    [HandlesResourceDisposal]
    public void Dispose()
    {
        char[]? toReturn = _charArray;
        this = default;
        TextPool.Return(toReturn);
    }

    public readonly override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is string str)
            return MemoryExtensions.Equals(str, _chars.Slice(0, _position), StringComparison.Ordinal);
        if (obj is char[] chars)
            return MemoryExtensions.Equals(chars, _chars.Slice(0, _position), StringComparison.Ordinal);
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly override int GetHashCode() => string.GetHashCode(_chars.Slice(0, _position), StringComparison.Ordinal);

    [HandlesResourceDisposal]
    public string ToStringAndDispose()
    {
        string str = this.ToString();
        this.Dispose();
        return str;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly override string ToString() => _chars.Slice(0, _position).ToString();
}