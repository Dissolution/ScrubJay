using ScrubJay.Universal;

namespace ScrubJay.Testing;

public interface IActual
{
    string? ValueName { get; }
    Type? ValueType { get; }
    string? ValueString { get; }
}

public interface IActual<out T> : IActual
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
{
    T Value { get; }

    IActual Realize();
}

[PublicAPI]
#if NET9_0_OR_GREATER
public ref struct Actual<T> : IActual<T>
    where T : allows ref struct
#else
public struct Actual<T> : IActual<T>
#endif
{
    public readonly T Value;

    public readonly string? ValueName;

    T IActual<T>.Value => Value;

    string? IActual.ValueName => ValueName;

    public string ValueString => Any.ToString(Value);

    public Type ValueType => Any.GetType(Value);

    internal Actual(T value, string? name)
    {
        this.Value = value;
        this.ValueName = name;
    }

    public IActual Realize()
    {
        return new RealizedActual(
            ValueType,
            ValueName,
            ValueString);
    }

    public override string ToString()
    {
        return $"{ValueName}: {ValueType} = {ValueString}";
    }
}

public sealed record class RealizedActual(Type ValueType, string? ValueName, string ValueString) : IActual;