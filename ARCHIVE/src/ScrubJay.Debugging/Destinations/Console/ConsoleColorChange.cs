namespace ScrubJay.Debugging.Destinations;

[PublicAPI]
[StructLayout(LayoutKind.Explicit, Size = 2, Pack = 0)]
public readonly struct ConsoleColorChange :
#if NET7_0_OR_GREATER
    IEqualityOperators<ConsoleColorChange, ConsoleColorChange, bool>,
#endif
    IEquatable<ConsoleColorChange>
{
    public static bool operator ==(ConsoleColorChange left, ConsoleColorChange right) => left.Equals(right);
    public static bool operator !=(ConsoleColorChange left, ConsoleColorChange right) => !left.Equals(right);

    public static implicit operator ConsoleColorChange(ConsoleColor? foreground) => new(foreground);
    
    private const byte CC_MASK = 0b00001111;
    private const byte USED_MASK = 0b10000000;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ConsoleColor? ByteToNCC(byte u8)
    {
        if ((u8 & USED_MASK) != 0)
            return (ConsoleColor)(u8 & CC_MASK);
        return null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static byte NCCToByte(ConsoleColor? color)
    {
        if (color.HasValue)
            return (byte)((byte)color.GetValueOrDefault() ^ USED_MASK);
        return 0;
    }
    
    [FieldOffset(0)]
    private readonly byte _foreground;
    
    [FieldOffset(1)]
    private readonly byte _background;

    public ConsoleColor? Foreground => ByteToNCC(_foreground);

    public ConsoleColor? Background => ByteToNCC(_background);

    private ConsoleColorChange(byte foreground, byte background)
    {
        _foreground = foreground;
        _background = background;
    }

    public ConsoleColorChange(ConsoleColor? foreground)
    {
        _foreground = NCCToByte(foreground);
        _background = 0;
    }

    public ConsoleColorChange(ConsoleColor? foreground, ConsoleColor? background)
    {
        _foreground = NCCToByte(foreground);
        _background = NCCToByte(background);
    }

    public void Deconstruct(out ConsoleColor? foreground, out ConsoleColor? background)
    {
        foreground = Foreground;
        background = Background;
    }

    public bool ChangesForeground(out ConsoleColor newForeground)
    {
        var fore = Foreground;
        newForeground = fore.GetValueOrDefault();
        return fore.HasValue;
    }
    
    public bool ChangesBackground(out ConsoleColor newBackground)
    {
        var back = Background;
        newBackground = back.GetValueOrDefault();
        return back.HasValue;
    }
    
    public ConsoleColorChange WithForeground(ConsoleColor? foreground)
    {
        return new ConsoleColorChange(NCCToByte(foreground), _background);
    }

    public ConsoleColorChange WithBackground(ConsoleColor? background)
    {
        return new ConsoleColorChange(_foreground, NCCToByte(background));
    }

    public bool Equals(ConsoleColorChange other) =>
        _foreground == other._foreground &&
        _background == other._background;

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is ConsoleColorChange ccc)
            return Equals(ccc);
        return false;
    }

    public override int GetHashCode() => _foreground ^ (_background << 8);

    public override string ToString()
    {
        return $"{Foreground.Name} / {Background.Name}";
    }
}