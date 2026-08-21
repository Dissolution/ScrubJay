namespace ScrubJay.Functional.IMPL;

/// <summary>
/// Represents the <b>Ok</b> portion of a <see cref="Result{T}"/> or <see cref="Result{T,E}"/>
/// </summary>
/// <remarks>
/// This class is primarily used for implicit coercions
/// </remarks>
[PublicAPI]
[StructLayout(LayoutKind.Auto)]
public readonly ref struct Ok<T>
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
{
    public readonly T Value;

    public Ok(T value)
    {
        Value = value;
    }

    public void Deconstruct(out T value)
    {
        value = Value;
    }
}