namespace ScrubJay.Text.Building;

[PublicAPI]
[InterpolatedStringHandler]
public ref struct InterpolatedTextBuilder : IDisposable
{
    internal readonly bool _passedBuilder;
    internal readonly TextBuilder _builder;
    
    public readonly int Length => _builder.Length;

    [MustDisposeResource(true)]
    public InterpolatedTextBuilder()
    {
        _passedBuilder = false;
        _builder = new TextBuilder();
    }

    [MustDisposeResource(true)]
    public InterpolatedTextBuilder(int literalLength, int formattedCount)
    {
        _passedBuilder = false;
        _builder = new TextBuilder(literalLength + (formattedCount * 16));
    }

    [MustDisposeResource(false)]
    public InterpolatedTextBuilder(TextBuilder builder)
    {
        _passedBuilder = true;
        _builder = builder ?? throw new ArgumentNullException(nameof(builder));
    }

    [MustDisposeResource(false)]
    public InterpolatedTextBuilder(int literalLength, int formattedCount, TextBuilder builder)
    {
        _passedBuilder = true;
        _builder = builder ?? throw new ArgumentNullException(nameof(builder));
    }

    public void AppendLiteral(string str) => _builder.Write(str);

    public void AppendFormatted(char ch) => _builder.Write(ch);

    public void AppendFormatted(char ch, int alignment)
        => throw new NotImplementedException();

    public void AppendFormatted(string? str) => _builder.Write(str);

    public void AppendFormatted(string? str, int alignment)
        => throw new NotImplementedException();

    public void AppendFormatted(scoped text text) => _builder.Write(text);

    public void AppendFormatted(scoped text text, int alignment)
        => throw new NotImplementedException();

    public void AppendFormatted<T>(T? value) => _builder.Append<T>(value);

#if NET9_0_OR_GREATER
    public void AppendFormatted<T>(in T? value, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        _builder.Append<T>(in value, _);
    }
#endif

    public void AppendFormatted<T>(T? value, string? format)
        => _builder.Format<T>(value, format);
    
    public void AppendFormatted<T>(T? value, scoped text format)
        => _builder.Format<T>(value, format);
    
    public void AppendFormatted(Action<TextBuilder>? build)
    {
        _builder.IndentAwareInvoke(build);
    }
    
    public void AppendFormatted<T>((Action<TextBuilder,T> BuildItem, T Value) tuple)
    {
        _builder.IndentAwareInvoke<T>(tuple.BuildItem, tuple.Value);
    }
    
    
    public override string ToString() => _builder.ToString();

    [HandlesResourceDisposal]
    public void Dispose()
    {
        if (!_passedBuilder)
        {
            _builder.Dispose();
        }
    }

    [HandlesResourceDisposal]
    public string ToStringAndDispose()
    {
        string str = this.ToString();
        this.Dispose();
        return str;
    }
}