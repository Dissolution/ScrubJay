using System.Buffers;

namespace ScrubJay.Text;

[PublicAPI]
public interface ITextWriter : IBufferWriter<char>
{
    void Write(char ch);
    void Write(string? str);
    void Write(text txt);
}