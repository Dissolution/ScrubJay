using System.Globalization;
using ScrubJay.Errors;
using ScrubJay.Functional;
using ScrubJay.Polyfills.Comparison;
using ScrubJay.Polyfills.Text;

namespace ScrubJay.Reflection;

/// <summary>
/// Represents an offset in IL
/// </summary>
[PublicAPI]
[StructLayout(LayoutKind.Explicit, Size = 2)]
public readonly struct ILOffset :
#if NET7_0_OR_GREATER
    IEqualityOperators<ILOffset, ILOffset, bool>,
    IComparisonOperators<ILOffset, ILOffset, bool>,
    ITrySpanParsable<ILOffset>,
    ITryParsable<ILOffset>,
#endif
#if NET6_0_OR_GREATER
    ISpanFormattable,
#endif
    IEquatable<ILOffset>,
    IComparable<ILOffset>,
    IFormattable
{
    public static implicit operator ILOffset(int offset) => new(offset);

    public static bool operator ==(ILOffset left, ILOffset right) => left.Equals(right);
    public static bool operator !=(ILOffset left, ILOffset right) => !left.Equals(right);
    public static bool operator >(ILOffset left, ILOffset right) => left.CompareTo(right) > 0;
    public static bool operator >=(ILOffset left, ILOffset right) => left.CompareTo(right) >= 0;
    public static bool operator <(ILOffset left, ILOffset right) => left.CompareTo(right) < 0;
    public static bool operator <=(ILOffset left, ILOffset right) => left.CompareTo(right) <= 0;

    public static readonly ILOffset Unknown = new ILOffset(-1);

    public static Result<ILOffset> TryParse(text text, IFormatProvider? provider = null)
    {
        var reader = new SpanReader<char>(text);

        // ignore leading whitespace
        reader.SkipWhile(char.IsWhiteSpace);

        // must start with "IL_"
        if (!reader.TryTakeMany(3, out var taken) || !Relate.Equate(taken, "IL_"))
            goto FAIL;

        // must then have 4 more characters
        if (!reader.TryTakeMany(4, out taken))
            goto FAIL;

        // they must be "????"
        if (Relate.Equate(taken, "????"))
            return ILOffset.Unknown;

        // or hexadecimal
        if (!taken.All(char.IsAsciiHexDigit))
            goto FAIL;

        // ignore trailing whitespace
        reader.SkipWhile(char.IsWhiteSpace);

        // that is all that is allowed
        if (!reader.IsCompleted)
            goto FAIL;

        // 4 hex digits = 16 bits = ushort
        if (ushort.TryParse(taken, NumberStyles.HexNumber, null, out ushort offset))
            return new ILOffset(offset);

        FAIL:
        return Ex.Parse<ILOffset>(text);
    }


    [FieldOffset(0)]
    private readonly short _offset;

    public bool IsUnknown
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _offset == -1;
    }

    public ILOffset(short offset)
    {
        if (offset >= 0)
        {
            _offset = offset;
        }
        else
        {
            _offset = -1;
        }
    }

    public ILOffset(int offset)
    {
        if (offset is >= 0 and <= short.MaxValue)
        {
            _offset = (short)offset;
        }
        else
        {
            _offset = -1;
        }
    }

    public int CompareTo(ILOffset offset) => _offset.CompareTo(offset._offset);

    public bool Equals(ILOffset offset) => _offset == offset._offset;

    public bool Equals(int offset) => _offset == offset;

    public bool Equals(short offset) => _offset == offset;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj switch
    {
        ILOffset ilOffset => Equals(ilOffset),
        int offset => Equals(offset),
        short offset => Equals(offset),
        _ => false,
    };

    public override int GetHashCode() => _offset;

    public bool TryFormat(Span<char> destination, out int charsWritten,
        text format = default,
        IFormatProvider? provider = default)
    {
        var writer = new TryFormatter(destination)
        {
            "IL_",
        };

        if (_offset >= 0)
        {
            writer.Add(_offset, format, provider);
        }
        else
        {
            writer.Add("????");
        }

        return writer.Wrote(out charsWritten);
    }

    public string ToString(string? format, IFormatProvider? provider = null)
    {
        using InterpolatedText interpolated = $"IL_";
        if (_offset >= 0)
        {
            interpolated.Format(_offset, format, provider);
        }
        else
        {
            interpolated.Write("????");
        }
        return interpolated.ToString();
    }

    public override string ToString() => ToString("X4", null);
}