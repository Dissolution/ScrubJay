namespace ScrubJay.Memory.Streaming;

public class ExplicitStreamWriter : IDisposable, IAsyncDisposable
{
    private readonly Stream _stream;
    private readonly bool _leaveOpen;
    
    public ExplicitStreamWriter(Stream stream, bool leaveOpen = false)
    {
        _stream = stream ?? throw new ArgumentNullException(nameof(stream));
        _leaveOpen = leaveOpen;
    }

    public void WriteAscii(scoped ReadOnlySpan<char> text)
    {
        int len = text.Length;
        Span<byte> bytes = ((len <= 1024) ? stackalloc byte[len] : new byte[len]);
        char ch;
        
        for (int i = 0; i < len; i++)
        {
            ch = text[i];
            if (!char.IsAscii(ch))
                throw new ArgumentException($"Contained a non-ASCII character: '{ch}'", nameof(text));
            bytes[i] = (byte)ch;
        }
        
        _stream.Write(bytes);
    }
    

    public void Dispose()
    {
        if (!_leaveOpen)
        {
            _stream.Dispose();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (!_leaveOpen)
        {
            await _stream.DisposeAsync();
        }
    }
}