namespace ScrubJay.Text.Building;

public partial class TextBuilder
{
    public TextBuilder Switch(Action<SwitchBuilder>? buildSwitch)
    {
        if (buildSwitch is not null)
        {
            var switchBuilder = new SwitchBuilder(this);
            buildSwitch(switchBuilder);
            return switchBuilder.Invoke();
        }
        return this;
    }
}

public class SwitchBuilder
{
    protected readonly TextBuilder _textBuilder;
    protected Action<TextBuilder>? _defaultCase;
    protected bool _invoked;

    internal SwitchBuilder(TextBuilder textBuilder)
    {
        _textBuilder = textBuilder;
    }

    public SwitchBuilder Case(bool condition, Action<TextBuilder>? onTrue)
    {
        if (!_invoked && condition)
        {
            onTrue?.Invoke(_textBuilder);
            _invoked = true;
        }
        return this;
    }

    public SwitchBuilder Case(Func<bool> predicate, Action<TextBuilder>? onTrue)
    {
        if (!_invoked && predicate())
        {
            onTrue?.Invoke(_textBuilder);
            _invoked = true;
        }
        return this;
    }

    public SwitchBuilder Case(Func<TextBuilder, bool>? predicate, Action<TextBuilder>? onTrue)
    {
        if (!_invoked && predicate is not null && predicate(_textBuilder))
        {
            onTrue?.Invoke(_textBuilder);
            _invoked = true;
        }
        return this;
    }

    public SwitchBuilder Case<T>(T value, Func<T, bool>? valuePredicate, Action<TextBuilder, T>? onTrue)
    {
        if (!_invoked && valuePredicate is not null && valuePredicate(value))
        {
            onTrue?.Invoke(_textBuilder, value);
            _invoked = true;
        }
        return this;
    }

    public SwitchBuilder Default(Action<TextBuilder>? onDefault)
    {
        if (!_invoked)
        {
            _defaultCase = onDefault;
        }
        return this;
    }

    internal TextBuilder Invoke()
    {
        if (!_invoked)
        {
            _defaultCase?.Invoke(_textBuilder);
        }
        return _textBuilder;
    }
}