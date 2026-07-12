namespace ScrubJay.Debugging.Destinations;

[PublicAPI]
[StructLayout(LayoutKind.Explicit, Size = 1, Pack = 0)]
public readonly struct ConsoleColors :
#if NET7_0_OR_GREATER
    IEqualityOperators<ConsoleColors, ConsoleColors, bool>,
#endif
    IEquatable<ConsoleColors>,
#if NET6_0_OR_GREATER
    ISpanFormattable,
#endif
    IFormattable
{
    public static implicit operator ConsoleColors(ConsoleColor foreground) => new(foreground);
    
    public static bool operator ==(ConsoleColors left, ConsoleColors right) => left.Equals(right);
    public static bool operator !=(ConsoleColors left, ConsoleColors right) => !left.Equals(right);

    
    
    private const byte FORE_MASK = 0b00001111; // low 4 bits
    private const byte BACK_MASK = 0b11110000; // high 4 bits
    private const int BACK_OFFSET = 4;

    [FieldOffset(0)]
    private readonly byte _colors;

    public ConsoleColor Foreground
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (ConsoleColor)(_colors & FORE_MASK);
    }

    public ConsoleColor Background
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (ConsoleColor)((_colors & BACK_MASK) >> BACK_OFFSET);
    }

    private ConsoleColors(byte colors)
    {
        _colors = colors;
    }

    public ConsoleColors(ConsoleColor foreground)
    {
        _colors = (byte)foreground;
    }

    public ConsoleColors(ConsoleColor foreground, ConsoleColor background)
    {
        _colors = (byte)((byte)foreground & ((byte)background << BACK_OFFSET));
    }

    public void Deconstruct(out ConsoleColor foreground, out ConsoleColor background)
    {
        foreground = Foreground;
        background = Background;
    }

    public ConsoleColors WithForeground(ConsoleColor foreground)
    {
        byte colors = (byte)((_colors & BACK_MASK) & (byte)foreground);
        return new ConsoleColors(colors);
    }

    public ConsoleColors WithBackground(ConsoleColor background)
    {
        byte colors = (byte)((_colors & FORE_MASK) & ((byte)background << BACK_OFFSET));
        return new ConsoleColors(colors);
    }

    public bool Equals(ConsoleColors other) => _colors == other._colors;

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is ConsoleColors consoleColors)
            return Equals(consoleColors);
        if (obj is ConsoleColor foreground)
            return Foreground == foreground;
        return false;
    }

    public override int GetHashCode() => _colors;

    public bool TryFormat(Span<char> destination, out int charsWritten, text format = default, IFormatProvider? provider = null)
    {
        if (format is "x")
            return _colors.TryFormat(destination, out charsWritten, "x2", provider);
        if (format is "X")
            return _colors.TryFormat(destination, out charsWritten, "X2", provider);

        if (format is "b" or "B")
        {
            if (destination.Length >= 8)
            {
                var binaryStr = _colors.ToBinaryString();
                binaryStr.CopyTo(destination);
                charsWritten = 8;
                return true;
            }

            goto FAIL;
        }

        var fn = Foreground.Name;
        if (!fn.TryCopyTo(destination))
            goto FAIL;
        charsWritten = fn.Length;

        if (!" / ".TryCopyTo(destination.Slice(charsWritten)))
            goto FAIL;
        charsWritten += 3;

        var bn = Background.Name;
        if (!bn.TryCopyTo(destination.Slice(charsWritten)))
            goto FAIL;
        charsWritten += bn.Length;
        return true;

        FAIL:
        charsWritten = 0;
        return false;
    }

    public string ToString(string? format, IFormatProvider? provider = null)
    {
        if (format is "x")
            return _colors.ToString("x2", provider);
        if (format is "X")
            return _colors.ToString("X2", provider);
        if (format is "b" or "B")
            return _colors.ToBinaryString();
        return ToString();
    }

    public override string ToString()
    {
        return $"{Foreground.Name} / {Background.Name}";
    }
}