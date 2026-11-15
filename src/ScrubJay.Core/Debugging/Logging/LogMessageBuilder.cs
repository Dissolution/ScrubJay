namespace ScrubJay.Debugging.Logging;

[PublicAPI]
[InterpolatedStringHandler]
[MustDisposeResource(true)]
public ref struct LogMessageBuilder : IDisposable
{
    private DefaultInterpolatedStringHandler _templateBuilder;
    private readonly List<(string? Name, object? Value)> _arguments;
    
    public LogMessageBuilder()
    {
        _templateBuilder = new DefaultInterpolatedStringHandler(1024, 0);
        _arguments = new(capacity: 0);
    }

    [HandlesResourceDisposal]
    public void Deconstruct(out string template, out LogMessageArguments arguments)
    {
        template = _templateBuilder.ToString();
        arguments = new(_arguments);
        Dispose();
    }

    public void AppendLiteral(string str)
    {
        _templateBuilder.AppendLiteral(str);
    }

    public void AppendFormatted(scoped text argument, [CallerArgumentExpression(nameof(argument))] string? argumentName = null)
    {
        _templateBuilder.AppendLiteral("{");
        _templateBuilder.AppendFormatted(_arguments.Count);
        _templateBuilder.AppendLiteral("}");
        _arguments.Add((Name: argumentName, Value: (object?)argument.ToString()));
    }

    public void AppendFormatted<T>(T? argument, [CallerArgumentExpression(nameof(argument))] string? argumentName = null)
    {
        _templateBuilder.AppendLiteral("{");
        _templateBuilder.AppendFormatted(_arguments.Count);
        _templateBuilder.AppendLiteral("}");
        _arguments.Add((Name: argumentName, Value: (object?)argument));
    }

    [HandlesResourceDisposal]
    public void Dispose()
    {
        _templateBuilder.Clear();
        _arguments.Clear();
    }

    public override string ToString()
    {
        var arguments = _arguments;
        var count = arguments.Count;
        object?[] args = new object?[count];
        for (var i = 0; i < count; i++)
        {
            args[i] = arguments[i].Value;
        }
        return string.Format(_templateBuilder.ToString(), args);
    }
}