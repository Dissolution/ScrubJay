using ScrubJay.Universal.Extensions;

namespace ScrubJay.Text.Building;

public partial class TextBuilder
{
    public TextBuilder Align(char ch, int width) => Align(ch, width, AlignmentOptions.Default);

    public TextBuilder Align(char ch, int width, AlignmentOptions options)
    {
        var (padChar, alignment, trim, truncateChar) = options;

        if (width == 0)
        {
            if (trim)
                return this;
            return Append(ch);
        }
        if (width == 1 || width == -1)
            return Append(ch);
        if (width < 0)
        {
            alignment |= Alignment.Left;
            width = -width;
        }

        int padding = width - 1;

        if (alignment == Alignment.Right)
        {
            return Repeat(padding, padChar).Append(ch);
        }

        // Use alignment
        if (alignment == Alignment.Left)
        {
            return Append(ch).Repeat(padding, padChar);
        }

        Debug.Assert(alignment.HasFlag(Alignment.Center));

        // if padding is even, pre + post are the same
        if (int.IsEvenInteger(padding))
        {
            int pad = padding / 2;
            return Repeat(pad, padChar)
                .Append(ch)
                .Repeat(pad, padChar);
        }

        // padding is odd, we need to use bias
        double half = padding / 2.0d;

        int pre;
        int post;

        // Center w/Left Bias?
        if (alignment.HasFlag(Alignment.Left))
        {
            pre = (int)Math.Floor(half);
            post = (int)Math.Ceiling(half);
        }
        else
        {
            // Defaults to Center w/Right Bias
            pre = (int)Math.Ceiling(half);
            post = (int)Math.Floor(half);
        }

        return Repeat(pre, padChar)
            .Append(ch)
            .Repeat(post, padChar);
    }

    public TextBuilder Align(scoped text text, int width) => Align(text, width, AlignmentOptions.Default);

    private void TruncateTextWithChar(scoped text text, int width, Alignment alignment, char indicator)
    {
        Debug.Assert(text.Length > width);
        Debug.Assert(Enum.IsDefined<Alignment>(alignment));

        // available width is one smaller
        width--;

        // only the indicator
        if (width == 0)
        {
            Write(indicator);
            return;
        }

        // right align, write the indicator and then the trailing text
        if (alignment == Alignment.Right)
        {
            Write(indicator);
            Write(text[^width..]);
            return;
        }

        // left align, write the starting text and then the indicator
        if (alignment == Alignment.Left)
        {
            Write(text[..width]);
            Write(indicator);
            return;
        }

        // center alignment requires two indicators
        Debug.Assert(alignment.HasFlag(Alignment.Center));
        // available width is one smaller again
        width--;

        // only the indicators
        if (width == 0)
        {
            Write(indicator);
            Write(indicator);
            return;
        }

        // get the center part of the text
        double mid = (text.Length / 2.0d) - (width / 2.0d);
        int start;
        if (alignment.HasFlag(Alignment.Left))
        {
            start = (int)Math.Floor(mid);
        }
        else
        {
            start = (int)Math.Ceiling(mid);
        }

        Write(indicator);
        Write(text.Slice(start, width));
        Write(indicator);
    }

    private void TruncateText(scoped text text, int width, Alignment alignment)
    {
        Debug.Assert(text.Length > width);
        Debug.Assert(Enum.IsDefined<Alignment>(alignment));

        // right align, write the trailing text
        if (alignment == Alignment.Right)
        {
            Write(text[^width..]);
            return;
        }

        // left align, write the starting text
        if (alignment == Alignment.Left)
        {
            Write(text[..width]);
            return;
        }


        Debug.Assert(alignment.HasFlag(Alignment.Center));

        // get the center part of the text
        double mid = (text.Length / 2.0d) - (width / 2.0d);
        int start;
        if (alignment.HasFlag(Alignment.Left))
        {
            start = (int)Math.Floor(mid);
        }
        else
        {
            start = (int)Math.Ceiling(mid);
        }

        Write(text.Slice(start, width));
    }

    public TextBuilder Align(
        scoped text text,
        int width,
        AlignmentOptions options)
    {
        var (padChar, alignment, trim, truncateChar) = options;

        if (width < 0)
        {
            alignment |= Alignment.Left;
            width = -width;
        }

        // fit text into width
        int textLength = text.Length;

        // no text: fill with padding
        if (textLength == 0)
        {
            return Repeat(width, padChar);
        }

        // not enough width
        if (width < textLength)
        {
            // we can show a truncated version of the text
            if (trim)
            {
                if (truncateChar.TryGetValue(out var indicator))
                {
                    TruncateTextWithChar(text, width, alignment, indicator);
                }
                else
                {
                    TruncateText(text, width, alignment);
                }
                return this;
            }
            else
            {
                // otherwise write all the text
                return Append(text);
            }
        }

        // calculate the amount of padding we have to add
        int padding = width - textLength;

        // Use alignment
        if (alignment == Alignment.Right)
        {
            return Repeat(padding, padChar).Append(text);
        }

        if (alignment == Alignment.Left)
        {
            return Append(text).Repeat(padding, padChar);
        }

        Debug.Assert(alignment.HasFlag(Alignment.Center));

        // if padding is even, pre + post are the same
        if (int.IsEvenInteger(padding))
        {
            int pad = padding / 2;
            return Repeat(pad, padChar)
                .Append(text)
                .Repeat(pad, padChar);
        }

        // padding is odd, we need to use bias
        double half = padding / 2.0d;

        int pre;
        int post;

        // Center w/Left Bias?
        if (alignment.HasFlag(Alignment.Left))
        {
            pre = (int)Math.Floor(half);
            post = (int)Math.Ceiling(half);
        }
        else
        {
            // Defaults to Center w/Right Bias
            pre = (int)Math.Ceiling(half);
            post = (int)Math.Floor(half);
        }

        return Repeat(pre, padChar)
            .Append(text)
            .Repeat(post, padChar);
    }

    public TextBuilder Align(Action<TextBuilder>? build, int width, AlignmentOptions options)
    {
        if (width == 0)
            return this;

        var (padChar, alignment, trim, truncateChar) = options;

        if (width < 0)
        {
            alignment |= Alignment.Left;
            width = -width;
        }

        // We have to build the text to know whe we have
        int start = _position;
        build?.Invoke(this);
        int end = _position;

        int textLength = end - start;

        // no text: fill with padding
        if (textLength == 0)
        {
            return Repeat(width, padChar);
        }

        // not enough width
        if (width < textLength)
        {
            // we can show a truncated version of the text
            if (trim)
            {
                throw new NotImplementedException();
            }
            else
            {
                // use everything we have written
                return this;
            }
        }

        // calculate the amount of padding we have to add
        int padding = width - textLength;

        // Use alignment
        int prePadding;
        int postPadding;

        if (alignment == Alignment.Right)
        {
            prePadding = padding;
            postPadding = 0;
        }
        else if (alignment == Alignment.Left)
        {
            prePadding = 0;
            postPadding = padding;
        }
        else
        {
            Debug.Assert(alignment.HasFlag(Alignment.Center));
            // if padding is even, pre + post are the same
            if (int.IsEvenInteger(padding))
            {
                prePadding = postPadding = padding / 2;
            }
            else
            {
                // padding is odd, we need to use bias
                double half = padding / 2.0d;

                // Center w/Left Bias?
                if (alignment.HasFlag(Alignment.Left))
                {
                    prePadding = (int)Math.Floor(half);
                    postPadding = (int)Math.Ceiling(half);
                }
                else
                {
                    // Defaults to Center w/Right Bias
                    prePadding = (int)Math.Ceiling(half);
                    postPadding = (int)Math.Floor(half);
                }
            }
        }

        if (prePadding > 0)
        {
            TryAllocateAt(start, prePadding, out var allocated);
            allocated.Fill(padChar);
        }

        if (postPadding > 0)
        {
            Repeat(prePadding, padChar);
        }

        return this;
    }
}