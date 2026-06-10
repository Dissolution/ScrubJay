namespace ScrubJay.Memory.Utilities;

/// <summary>
/// Indicates what kind of prefix or postfix that will be used to determine the length of the resulting string
/// </summary>
[PublicAPI]
public enum StringEncodingAffix
{
    None,
    SevenBitEncodedLenPrefix,
    U8Prefix,
    U16Prefix,
    U32Prefix,
    U64Prefix,
    I8Prefix,
    I16Prefix,
    I32Prefix,
    I64Prefix,
    NullTerminated,
}