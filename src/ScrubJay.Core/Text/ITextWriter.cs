using System.Buffers;

namespace ScrubJay.Text;

[PublicAPI]
public interface ITextWriter : IBufferWriter<char>
{
    [OverloadResolutionPriority(100)]
    void Write(char ch);

    [OverloadResolutionPriority(90)]
    void Write(text txt);

    [OverloadResolutionPriority(80)]
    void Write(string? str) => Write(str.AsSpan());

    [OverloadResolutionPriority(70)]
    void Write<T>(T? value) => Write(value?.ToString());

#if NET9_0_OR_GREATER
    [OverloadResolutionPriority(70)]
    void Write<T>(T? value, Constrain.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
        => Write(Anything.ToString(value));
#endif


    void WriteFormatted<T>(T? value, string? format = null, IFormatProvider? provider = null);
}