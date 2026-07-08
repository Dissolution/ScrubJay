//
//static class Sink
//{
//    // NOTE: initialBuffer forwarded to the handler ctor via InterpolatedStringHandlerArgument
//    public static string Format(Span<char> initialBuffer,
//        [InterpolatedStringHandlerArgument("initialBuffer")] PooledHandler h)
//    {
//        string s = h.Text.ToString();
//        h.Dispose();
//        return s;
//    }
//}
//
//[InterpolatedStringHandler]
//public ref struct PooledHandler
//{
//    private Span<char> _chars;
//    private char[]? _arrayToReturnToPool;
//    private int _pos;
//
//    // top-level ctor with a caller-provided (possibly stackalloc) scratch buffer
//    public PooledHandler(int literalLength, int formattedCount, Span<char> initialBuffer)
//    {
//        _chars = initialBuffer;
//        _arrayToReturnToPool = null;
//        _pos = 0;
//    }
//
//    // nested ctor: receiver forwarded by value
//    public PooledHandler(int literalLength, int formattedCount, PooledHandler outer)
//    {
//        _chars = outer._chars;
//        _arrayToReturnToPool = outer._arrayToReturnToPool;
//        _pos = outer._pos;
//    }
//
//    private void Grow(int additional)
//    {
//        int needed = _pos + additional;
//        char[] next = ArrayPool<char>.Shared.Rent(Math.Max(needed, _chars.Length * 2));
//        _chars.Slice(0, _pos).CopyTo(next);
//        char[]? toReturn = _arrayToReturnToPool;
//        _chars = _arrayToReturnToPool = next;
//        if (toReturn is not null) ArrayPool<char>.Shared.Return(toReturn);
//    }
//
//    public void AppendLiteral(string s)
//    {
//        if (s.TryCopyTo(_chars.Slice(_pos))) { _pos += s.Length; return; }
//        Grow(s.Length);
//        s.CopyTo(_chars.Slice(_pos));
//        _pos += s.Length;
//    }
//
//    public void AppendFormatted<T>(T value) => AppendLiteral(value?.ToString() ?? "");
//

//    public ReadOnlySpan<char> Text => _chars.Slice(0, _pos);
