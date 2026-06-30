using ScrubJay.Universal;

namespace ScrubJay.Errors.Arguments;

[PublicAPI]
public readonly ref struct Argument<T>
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
{
    public readonly Type Type;
    public readonly string? Name;
    public readonly T? Value;

    internal Argument(Type type, string? name, in T? value)
    {
        Type = type;
        Name = name;
        Value = value;
    }

    public override string ToString()
    {
        return $"\"{Name}\": {Type} = {Any.ToString(in Value)}";
    }
}