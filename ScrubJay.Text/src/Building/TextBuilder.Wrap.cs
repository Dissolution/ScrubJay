namespace ScrubJay.Text.Building;

public partial class TextBuilder
{
    public TextBuilder Wrap(char wrapChar, Action<TextBuilder>? build)
    {
        Write(wrapChar);
        build?.Invoke(this);
        Write(wrapChar);
        return this;
    }

    public TextBuilder Wrap(string? wrapString, Action<TextBuilder>? build)
    {
        Write(wrapString);
        build?.Invoke(this);
        Write(wrapString);
        return this;
    }

    public TextBuilder Wrap<W>(W? wrapValue, Action<TextBuilder>? build)
    {
        Write<W>(wrapValue);
        build?.Invoke(this);
        Write<W>(wrapValue);
        return this;
    }

    public TextBuilder Wrap(char pre, char post, Action<TextBuilder>? build)
    {
        Write(pre);
        build?.Invoke(this);
        Write(post);
        return this;
    }

    public TextBuilder Wrap(string? pre, string? post, Action<TextBuilder>? build)
    {
        Write(pre);
        build?.Invoke(this);
        Write(post);
        return this;
    }

    public TextBuilder Wrap<TPre, TPost>(TPre? pre, TPost? post, Action<TextBuilder>? build)
    {
        Write<TPre>(pre);
        build?.Invoke(this);
        Write<TPost>(post);
        return this;
    }

    public TextBuilder Wrap((char Pre, char Post) wrap, Action<TextBuilder>? build)
    {
        Write(wrap.Pre);
        build?.Invoke(this);
        Write(wrap.Post);
        return this;
    }

    public TextBuilder Wrap((string? Pre, string? Post) wrap, Action<TextBuilder>? build)
    {
        Write(wrap.Pre);
        build?.Invoke(this);
        Write(wrap.Post);
        return this;
    }

    public TextBuilder Wrap<TPre, TPost>((TPre? Pre, TPost? Post) wrap, Action<TextBuilder>? build)
    {
        Write<TPre>(wrap.Pre);
        build?.Invoke(this);
        Write<TPost>(wrap.Post);
        return this;
    }
}