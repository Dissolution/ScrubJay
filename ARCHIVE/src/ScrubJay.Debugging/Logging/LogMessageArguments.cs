namespace ScrubJay.Debugging.Logging;

[PublicAPI]
public sealed class LogMessageArguments :
    IReadOnlyDictionary<string?, object?>,
    IReadOnlyList<object?>
{
    private readonly (string? Name, object? Value)[] _arguments;

    IEnumerable<string?> IReadOnlyDictionary<string?, object?>.Keys => _arguments.Select(static arg => arg.Name);

    IEnumerable<object?> IReadOnlyDictionary<string?, object?>.Values => _arguments.Select(static arg => arg.Value);


    public object? this[string? key] => TryGetValue(key, out var value) ? value : throw new ArgumentOutOfRangeException(nameof(key), key, null);

    public object? this[int index] => _arguments[index].Value;

    public int Count => _arguments.Length;


    public LogMessageArguments()
    {
        _arguments = [];
    }

    public LogMessageArguments(IEnumerable<(string?, object?)>? arguments)
    {
        if (arguments is not null)
        {
            _arguments = arguments.ToArray();
        }
        else
        {
            _arguments = [];
        }
    }

    bool IReadOnlyDictionary<string?, object?>.ContainsKey(string? argumentName)
    {
        return _arguments.Any(arg => string.Equals(arg.Name, argumentName, StringComparison.Ordinal));
    }

    public bool TryGetValue(string? name, [MaybeNullWhen(false)] out object? value)
    {
        foreach (var arg in _arguments)
        {
            if (string.Equals(arg.Name, name, StringComparison.Ordinal))
            {
                value = arg.Value;
                return true;
            }
        }

        value = null;
        return false;
    }

    IEnumerator IEnumerable.GetEnumerator()
        => _arguments
            .Select(static arg => arg.Value)
            .GetEnumerator();

    IEnumerator<object?> IEnumerable<object?>.GetEnumerator()
        => _arguments
            .Select(static arg => arg.Value)
            .GetEnumerator();

    IEnumerator<KeyValuePair<string?, object?>> IEnumerable<KeyValuePair<string?, object?>>.GetEnumerator()
        => _arguments
            .Select(static arg => new KeyValuePair<string?, object?>(arg.Name, arg.Value))
            .GetEnumerator();

    public IEnumerator<(string? Name, object? Value)> GetEnumerator() => ((IEnumerable<(string? Name, object? Value)>)_arguments).GetEnumerator();

    public override string ToString()
    {
        var message = new StringBuilder();
        message.AppendLine("{");
        foreach (var arg in _arguments)
        {
            message.AppendLine()
                .Append("  ").Append(arg.Name).Append(": ").Append(arg.Value);
        }
        message.Append('}');
        return message.ToString();
    }
}