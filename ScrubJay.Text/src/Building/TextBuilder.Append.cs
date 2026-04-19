namespace ScrubJay.Text.Building;

public partial class TextBuilder
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TextBuilder Append(char ch)
    {
        Write(ch);
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TextBuilder Append(scoped text text)
    {
        Write(text);
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TextBuilder Append(string? str)
    {
        Write(str);
        return this;
    }

    public TextBuilder Append(char[]? charArray)
    {
        Write(charArray);
        return this;
    }

    public TextBuilder Append(ICollection<char>? chars)
    {
        if (chars is not null)
        {
            MaybeGrowBy(chars.Count);
            chars.CopyTo(_chars, _position);
            _position += chars.Count;
        }
        return this;
    }

    public TextBuilder Append(
        [InterpolatedStringHandlerArgument("")]
        ref InterpolatedTextBuilder interpolatedTextBuilder)
    {
        // writing has already happened
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TextBuilder Append<T>(T? value)
    {
        Write<T>(value);
        return this;
    }

#if NET9_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // ReSharper disable once MethodOverloadWithOptionalParameter
    public TextBuilder Append<T>(in T? value, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        Write<T>(in value, _);
        return this;
    }
#endif
}