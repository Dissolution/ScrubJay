namespace ScrubJay.Memory.Collections;

/// <summary>
/// Represents a concrete readonly slice of bytes.
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public readonly struct Bytes : 
    IReadOnlyList<byte>,
    IReadOnlyCollection<byte>,
    IEnumerable<byte>,
    IFormattable
{
    public static implicit operator Bytes(byte[]? bytes) => new(bytes);
    public static implicit operator Bytes(byte u8) => new([u8]);

    public static readonly Bytes Empty = default;

    private readonly byte[] _bytes;

    public byte this[int index] => _bytes[index];

    public int Count => _bytes.Length;

    public Bytes()
    {
        _bytes = [];
    }

    public Bytes(byte[]? bytes)
    {
        _bytes = bytes ?? [];
    }

    internal string DebuggerDisplay() => $"""
        {Count} Bytes
        Hex: {ToString("X2")}
        ASCII: {ToString("ascii")}
        """;

    public string ToString(Encoding? encoding)
    {
        if (encoding is null)
            return ToString();

        try
        {
            return encoding.GetString(_bytes);
        }
        catch
        {
            return $"Invalid {encoding.EncodingName} bytes";
        }
    }

    public string ToString(string? format, IFormatProvider? _ = default)
    {
        if (string.IsNullOrEmpty(format))
            return ToString();

        if (string.Equals(format, "ascii", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(format, "a", StringComparison.OrdinalIgnoreCase))
        {
            return ToString(Encoding.ASCII);
        }
        if (string.Equals(format, "utf8", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(format, "8", StringComparison.OrdinalIgnoreCase))
        {
            return ToString(Encoding.UTF8);
        }
        if (string.Equals(format, "utf16", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(format, "unicode", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(format, "16", StringComparison.OrdinalIgnoreCase))
        {
            return ToString(Encoding.Unicode);
        }
        if (string.Equals(format, "utf32", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(format, "32", StringComparison.OrdinalIgnoreCase))
        {
            return ToString(Encoding.UTF32);
        }

        if (string.Equals(format, "bigint", StringComparison.OrdinalIgnoreCase))
        {
            var bigint = new BigInteger(_bytes);
            return bigint.ToString();
        }

        // fallback to passthrough format

        var bytes = _bytes;
        int count = Count;
        DefaultInterpolatedStringHandler text = new(2 + (count - 1), count);
        text.AppendLiteral("[");
        if (count > 0)
        {
            text.AppendFormatted(bytes[0], format);
            for (int i = 1; i < count; i++)
            {
                text.AppendLiteral(" ");
                text.AppendFormatted(bytes[i], format);
            }
        }
        text.AppendLiteral("]");
        return text.ToStringAndClear();
    }

    public override string ToString()
    {
        var bytes = _bytes;
        int count = Count;
        DefaultInterpolatedStringHandler text = new(3 * count + 1, 0);
        text.AppendLiteral("[");
        if (count > 0)
        {
            text.AppendFormatted(bytes[0], "X2");
            for (int i = 1; i < count; i++)
            {
                text.AppendLiteral(" ");
                text.AppendFormatted(bytes[i], "X2");
            }
        }
        text.AppendLiteral("]");
        return text.ToStringAndClear();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    IEnumerator<byte> IEnumerable<byte>.GetEnumerator() => GetEnumerator();

    public ByteArrayEnumerator GetEnumerator() => new(_bytes);
}

