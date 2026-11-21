using ScrubJay.Comparison;
using ScrubJay.Errors.Arguments;
using ScrubJay.Text;

namespace ScrubJay.Debugging.Logging;

[PublicAPI]
[InterpolatedStringHandler]
[MustDisposeResource(true)]
[StructLayout(LayoutKind.Auto)]
public ref struct LogMessageBuilder : IDisposable
{
    internal Interpolator _template;
    internal readonly List<Argument> _arguments;

    public LogMessageBuilder()
    {
        _template = new Interpolator();
        _arguments = new(capacity: 0);
    }

    [HandlesResourceDisposal]
    internal void Deconstruct(out CompositeFormat template, out List<Argument> arguments)
    {
        template = CompositeFormat.Parse(_template.ToStringAndDispose());
        arguments = _arguments;
    }

    public void AppendLiteral(string str)
    {
        _template.Write(str);
    }

    public void AppendFormatted<T>(T? value, [CallerArgumentExpression(nameof(value))] string? valueName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        int argIndex = -1;

        if (valueName is null)
        {
            argIndex = _arguments.Count;
            valueName = $"Arg{argIndex}";
            _arguments.Add(Argument.Capture(value, valueName));
        }
        else
        {
            valueName = valueName.Replace('{', '[').Replace('}', ']');

            for (var i = 0; i < _arguments.Count; i++)
            {
                var arg = _arguments[i];
                if (arg.Name == valueName && Relate.Equate(value, arg.Value))
                {
                    argIndex = i;
                    break;
                }
            }

            if (argIndex == -1)
            {
                argIndex = _arguments.Count;
                _arguments.Add(Argument.Capture(value, valueName));
            }
        }

        _template.Write('{');
        _template.Write(argIndex);
        _template.Write('}');
    }

    [HandlesResourceDisposal]
    public void Dispose()
    {
        _template.Dispose();
        _arguments.Clear();
    }

    public override string ToString()
    {
        //var template = _template.Written;
        // todo
        return _template.ToString();
    }
}