namespace ScrubJay.Testing.Naughty;

/// <summary>
/// A boxable recipe for a value that cannot be boxed. <see cref="Run"/> builds the value and passes it to the
/// consumer, which calls the code under test.
/// </summary>
public sealed class NaughtyCase<TConsumer>
    where TConsumer : Delegate
{
    private readonly Action<TConsumer> _run;

    public NaughtyCase(string category, string name, string why, Action<TConsumer> run)
    {
        Category = category;
        Name = name;
        Why = why;
        _run = run;
    }

    public string Category { get; }

    public string Name { get; }

    public string Why { get; }

    public void Run(TConsumer consumer) => _run(consumer);

    public override string ToString() => $"{Category}: {Name}";
}