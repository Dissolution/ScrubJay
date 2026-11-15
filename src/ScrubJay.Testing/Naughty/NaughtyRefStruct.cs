namespace ScrubJay.Testing.Naughty;

/// <summary>
/// A ref struct whose default instance holds a null ref field, so reading <see cref="Value"/> throws
/// <see cref="NullReferenceException"/>. Use it as a type argument for generics marked <c>allows ref struct</c>.
/// </summary>
public ref struct NaughtyRefStruct
{
    public ref int Value;

    public NaughtyRefStruct(ref int value)
    {
        Value = ref value;
    }
}