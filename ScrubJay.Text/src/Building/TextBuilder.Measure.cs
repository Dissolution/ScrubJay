namespace ScrubJay.Text.Building;

partial class TextBuilder
{
    /// <summary>
    /// Measures the number of characters written during a <see cref="TB"/>.
    /// </summary>
    /// <param name="build"></param>
    /// <param name="charsWritten"></param>
    /// <returns></returns>
    public TextBuilder Measure(Action<TextBuilder>? build, out int charsWritten)
    {
        if (build is not null)
        {
            int start = _position;
            build(this);
            charsWritten = _position - start;
        }
        else
        {
            charsWritten = 0;
        }
        return this;
    }
    
    public TextBuilder Measure<S>(S state, Action<TextBuilder, S>? statefulBuild, out int charsWritten)
#if NET9_0_OR_GREATER
        where S : allows ref struct
#endif
    {
        if (statefulBuild is not null)
        {
            int start = _position;
            statefulBuild(this, state);
            charsWritten = _position - start;
        }
        else
        {
            charsWritten = 0;
        }
        return this;
    }
    
    public TextBuilder Capture(Action<TextBuilder>? build, out Span<char> written)
    {
        if (build is not null)
        {
            int start = _position;
            build(this);
            written = _chars.AsSpan(start, _position - start);
        }
        else
        {
            written = default;
        }

        return this;
    }

    public TextBuilder Capture<S>(S state, Action<TextBuilder, S>? statefulBuild, out Span<char> written)
#if NET9_0_OR_GREATER
        where S : allows ref struct
#endif
    {
        if (statefulBuild is not null)
        {
            int start = _position;
            statefulBuild(this, state);
            written = _chars.AsSpan(start, _position - start);
        }
        else
        {
            written = default;
        }

        return this;
    }
}