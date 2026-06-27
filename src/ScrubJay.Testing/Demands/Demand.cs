using ScrubJay.Polyfills.Universal;

namespace ScrubJay.Testing.Demands;

public class Demand
{

}

public readonly ref struct Argument<T>
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
{
    public readonly Type Type;
    public readonly string? Name;
    public readonly T Value;
    public readonly string? ValueString;

    public Argument(Type? type, string? name, T value)
    {
        this.Type = type ?? typeof(T);
        this.Name = name;
        this.Value = value;
        this.ValueString = Any.ToString<T>(in value);
    }
}