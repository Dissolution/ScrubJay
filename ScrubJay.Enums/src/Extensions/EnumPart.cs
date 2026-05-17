namespace ScrubJay.Enums.Extensions;

/// <summary>
/// Specifies a particular part of an <see langword="enum"/> member.
/// </summary>
[PublicAPI]
public enum EnumPart
{
    /// <summary>
    /// The name of the member (as it was declared)
    /// </summary>
    Name,

    /// <summary>
    /// The underlying value of the member
    /// </summary>
    Value,

    /// <summary>
    /// A defined <see cref="Attribute"/> format
    /// </summary>
    Attribute,
}