using ScrubJay.Text.Collections;

namespace ScrubJay.Text.Building;

[PublicAPI]
[MustDisposeResource(true)]
public sealed class Whitespace : IDisposable
{
    private char[] _whitespace;
    private int _offset;

    private readonly MiniStack _indentOffsets;

    [JetBrains.Annotations.NotNull, AllowNull]
    public string CurrentNewLine
    {
        get;
        set => SetNewLine(ref field, value);
    } = WhitespaceManager.DefaultNewLine;

    [JetBrains.Annotations.NotNull, AllowNull]
    public string CurrentDefaultIndent
    {
        get;
        set => field = value ?? WhitespaceManager.DefaultIndent;
    } = WhitespaceManager.DefaultIndent;

    public text FullNewLine => _whitespace.AsSpan(0, _offset);
    public string FullNewLineString => new string(_whitespace, 0, _offset);

    public text OutdentNewLine
    {
        get
        {
            if (_indentOffsets.TryPeek(out int previousOffset))
            {
                return _whitespace.AsSpan(0, previousOffset);
            }
            return FullNewLine;
        }
    }
    public text IndentsOnly => FullNewLine[CurrentNewLine.Length..];

    public int IndentCount => _indentOffsets.Count;

    public Whitespace()
    {
        _whitespace = TextPool.Rent(16);
        _offset = CurrentNewLine.Length;
        TextHelper.Unsafe.CopyTo(CurrentNewLine, _whitespace, _offset);
        _indentOffsets = new();
    }

    private void SetNewLine(ref string field, string? newline)
    {
        if (newline is null)
        {
            field = WhitespaceManager.DefaultNewLine;
            return;
        }

        int oldLength = field.Length;
        int newLength = newline.Length;

        // We need to account for a change in length
        int delta = newLength - oldLength;

        // the same => swap the newlines in place, done
        if (delta == 0)
        {
            // If they are the same, we can swap in-place
            TextHelper.Unsafe.CopyTo(newline, _whitespace, newLength);
            field = newline;
            return;
        }

        var newArray = TextPool.Rent(_offset + Math.Abs(delta));
        TextHelper.Unsafe.CopyTo(newline, newArray, newLength);
        _whitespace
            .AsSpan(oldLength, _offset - oldLength)
            .CopyTo(newArray.AsSpan(newLength));
        TextPool.Return(_whitespace);
        _whitespace = newArray;
        _indentOffsets.OffsetAll(delta);
        _offset += delta;
        field = newline;
    }

    public void AddIndent(scoped text indent)
    {
        // what we store in the stack is the starting offset for this indent
        // we know the ending by _offset

        int offset = _offset;
        _indentOffsets.Push(offset);
        int indentLength = indent.Length;
        int newOffset = offset + indentLength;
        if (newOffset > _whitespace.Length)
        {
            TextPool.GrowBy(ref _whitespace!, indentLength);
        }
        TextHelper.Unsafe.CopyTo(indent, _whitespace.AsSpan(offset), indentLength);
        _offset = newOffset;
    }

    public void AddIndent(string? indent = null)
    {
        // what we store in the stack is the starting offset for this indent
        // we know the ending by _offset

        int offset = _offset;
        _indentOffsets.Push(offset);

        indent ??= CurrentDefaultIndent;
        int indentLength = indent.Length;
        int newOffset = offset + indentLength;
        if (newOffset > _whitespace.Length)
            TextPool.GrowBy(ref _whitespace!, indentLength);
        TextHelper.Unsafe.CopyTo(indent, _whitespace.AsSpan(offset), indentLength);
        _offset = newOffset;
    }

    public bool TryPeekLastIndent([NotNullWhen(true)] out string? lastIndent)
    {
        if (_indentOffsets.TryPeek(out int lastOffset))
        {
            lastIndent = new string(_whitespace, lastOffset, _offset - lastOffset);
            return true;
        }
        lastIndent = null;
        return false;
    }

    public bool TryRemoveIndent()
    {
        if (_indentOffsets.TryPop(out int newOffset))
        {
            _offset = newOffset;
            return true;
        }
        return false;
    }

    public bool TryRemoveIndent([NotNullWhen(true)] out string? indent)
    {
        if (_indentOffsets.TryPop(out int newOffset))
        {
            indent = _whitespace.AsSpan(newOffset, _offset - newOffset).ToString();
            _offset = newOffset;
            return true;
        }
        indent = null;
        return false;
    }

    public void Dispose()
    {
        _offset = 0;
        _indentOffsets.Clear();
        var toReturn = Interlocked.Exchange(ref _whitespace, []);
        TextPool.Return(toReturn);
    }
}