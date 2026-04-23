using ScrubJay.Text.Collections;

namespace ScrubJay.Text.Building;

[PublicAPI]
public static class WhitespaceManager
{
    /// <summary>
    /// Gets or sets the default <see cref="string"/> NewLine that <see cref="WhitespaceManager"/> instances will use.
    /// </summary>
    [AllowNull, NotNull]
    public static string DefaultNewLine
    {
        get => field;
        set => field = value ?? Environment.NewLine;
    } = Environment.NewLine;

    /// <summary>
    /// Gets or sets the default <see cref="string"/> Indent that <see cref="WhitespaceManager"/> instances will use.
    /// </summary>
    [AllowNull, NotNull]
    public static string DefaultIndent
    {
        get => field;
        set => field = value ?? "    ";
    } = "    "; // 4 spaces
}

[PublicAPI]
[MustDisposeResource(true)]
public sealed class Whitespace : IDisposable
{
    private string _newline = WhitespaceManager.DefaultNewLine;
    private string _defaultIndent = WhitespaceManager.DefaultIndent;
    
    private char[] _whitespace;
    private int _offset;
    
    private readonly MiniStack _indentOffsets;

    [NotNull, AllowNull]
    public string NewLine
    {
        get => _newline;
        set => SetNewLine(ref _newline, value);
    }

    [NotNull, AllowNull]
    public string DefaultIndent
    {
        get => _defaultIndent;
        set => _defaultIndent = value ?? WhitespaceManager.DefaultIndent;
    }

    public text FullNewLine => _whitespace.AsSpan(0, _offset);
    public text IndentsOnly => FullNewLine.Slice(_newline.Length);
    
    public Whitespace()
    {
        _whitespace = TextPool.Rent(16);
        _offset = _newline.Length;
        TextHelper.Unsafe.CopyTo(_newline, _whitespace, _offset);
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
    
    
    

  

    internal bool IsStartLine(TextBuilder builder)
    {
        if (builder.Length == 0)
            return true;

        var nli = _whitespace.Written;
        if (builder.Length < nli.Length)
            return false;
        return builder.Written.EndsWith(nli);
    }

    internal bool IsStartDedentedLine(TextBuilder builder)
    {
        if (builder.Length == 0)
            return true;

        var nli = _whitespace.Written;
        if (_indentOffsets.Count > 0)
        {
            nli = nli[.._indentOffsets[^1]];
        }

        if (builder.Length < nli.Length)
            return false;
        return builder.Written.EndsWith(nli);
    }

    public void AddIndent(string? indent = null)
    {
        int offset = _whitespace.Count;
        _whitespace.AddMany(indent ?? Indent);
        _indentOffsets.Add(offset);
    }

    public void RemoveIndent()
    {
        if (_indentOffsets.Count == 0)
            throw Ex.Invalid("There are no indents to remove");

        int offset = _indentOffsets[^1];
        _indentOffsets.TryRemoveAt(^1);
        _whitespace.Count = offset;
    }

    public void Dispose()
    {
        _indentOffsets.Dispose();
        _whitespace.Dispose();
    }
}