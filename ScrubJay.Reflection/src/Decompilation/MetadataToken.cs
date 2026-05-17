using ScrubJay.Text.Building;
using ScrubJay.Text.Rendering;

namespace ScrubJay.Reflection.Decompilation;

/// <summary>
/// 
/// </summary>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.reflection.memberinfo.metadatatoken"/>
/// 
[PublicAPI]
[StructLayout(LayoutKind.Explicit, Size = 4)]
public readonly struct MetadataToken :
#if NET7_0_OR_GREATER
    IEqualityOperators<MetadataToken, MetadataToken, bool>,
#endif
#if NET6_0_OR_GREATER
    ISpanFormattable,
#endif
    IEquatable<MetadataToken>,
    IFormattable,
    IRenderable
{
    private const uint RID_MASK = 0b00000000_11111111_11111111_11111111;

    public static implicit operator MetadataToken(int token) => new(token);
    public static implicit operator int(MetadataToken token) => token.I32Value;

    public static bool operator ==(MetadataToken left, MetadataToken right) => left.Equals(right);
    public static bool operator !=(MetadataToken left, MetadataToken right) => !left.Equals(right);

    public static readonly MetadataToken Empty;


    [FieldOffset(0)]
    private readonly uint _token;

    public MetadataTokenType MetadataTokenType
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (MetadataTokenType)(_token >> (8 * 3));
    }

    public uint Identifier
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _token & RID_MASK;
    }

    public bool IsEmpty
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _token == 0U;
    }

    public int I32Value
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => unchecked((int)_token);
    }

    public uint U32Value
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _token;
    }

    public MetadataToken(int token)
    {
        _token = (uint)token;
    }

    public bool Equals(MetadataToken token) => token._token == _token;

    public bool Equals(int token) => token == _token;

    public bool Equals(uint token) => token == _token;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj switch
    {
        MetadataToken metadataToken => Equals(metadataToken),
        int i32Token => Equals(i32Token),
        uint u32Token => Equals(u32Token),
        _ => false,
    };

    public override int GetHashCode() => I32Value;

    public bool TryFormat(Span<char> destination, out int charsWritten,
        text format = default,
        IFormatProvider? provider = default)
    {
        return new TryFormatWriter(destination)
        {
            { _token, format, provider },
        }.Wrote(out charsWritten);
    }

    public void RenderTo(TextBuilder builder) => builder
        .Render(MetadataTokenType)
        .Append('.')
        .Format(Identifier, "X6");

    public string ToString(string? format, IFormatProvider? provider = null) => _token.ToString(format, provider);

    public override string ToString() => TextBuilder.Build(RenderTo);
}