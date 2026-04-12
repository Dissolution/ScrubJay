using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis.CSharp;

namespace ScrubJay.Enums.SourceGen.Coding;

public partial class CodeBuilder : IDisposable
{
    private static readonly string _newline = SyntaxFactory.ElasticCarriageReturnLineFeed.ToString();

    private static char[] RentCharArray(int minCapacity)
    {
        return ArrayPool<char>.Shared.Rent(Math.Max(minCapacity, 1024));
    }

    private static void ReturnCharArray(char[]? array)
    {
        if (array is not null && array.Length > 0)
        {
            ArrayPool<char>.Shared.Return(array, true);
        }
    }


    private char[] _charArray;
    private int _position = 0;
    private int _indentCount = 0;

    internal Span<char> Written
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _charArray.AsSpan(0, _position);
    }

    internal Span<char> Available
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _charArray.AsSpan(_position);
    }

    internal int Capacity
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _charArray.Length;
    }

    public CodeBuilder()
    {
        _charArray = [];
    }

    public CodeBuilder(int minCapacity)
    {
        _charArray = RentCharArray(minCapacity);
    }

    private void GrowBy(int count)
    {
        Debug.Assert(count > 0);
        var newArray = RentCharArray((Capacity + count) * 2);
        _charArray.AsSpan(0, _position).CopyTo(newArray);
        var toReturn = _charArray;
        _charArray = newArray;
        ReturnCharArray(toReturn);
    }

    private void GrowTo(int minCapacity)
    {
        var newArray = RentCharArray(minCapacity);
        _charArray.AsSpan(0, _position).CopyTo(newArray);
        var toReturn = _charArray;
        _charArray = newArray;
        ReturnCharArray(toReturn);
    }


    public void EnsureCapacity(int minCapacity)
    {
        if (minCapacity > Capacity)
        {
            GrowTo(minCapacity);
        }
    }

    internal void Adding(int count)
    {
        int newCapacity = _position + count;
        if (newCapacity > Capacity)
        {
            GrowTo(newCapacity);
        }
    }

    public void Write(char ch)
    {
        if (_position >= Capacity)
            GrowBy(1);
        _charArray[_position++] = ch;
    }

    public void Write(scoped ReadOnlySpan<char> text)
    {
        int textLen = text.Length;
        if (textLen > 0)
        {
            int pos = _position;
            int newPos = pos + textLen;
            if (newPos >= Capacity)
            {
                GrowBy(textLen);
            }
            text.CopyTo(_charArray.AsSpan(pos));
            _position = newPos;
        }
    }

    
    internal void InterpolatedDelegate<T>(CodeBuilderValueAction<T> action, T value)
    {
        var oldIndent = _indentCount;
        var currentIndent = GetCurrentIndent();
        _indentCount = currentIndent;
        action.Invoke(this, value);
        _indentCount = oldIndent;
    }

#region Indents
    public void AddIndent()
    {
        _indentCount++;
    }

    public void RemoveIndent()
    {
        if (_indentCount == 0)
            throw new InvalidOperationException("No indent to remove");
        _indentCount--;
    }

    public void SetIndent(int indent)
    {
        if (indent <= 0)
        {
            _indentCount = 0;
        }
        else
        {
            _indentCount = indent;
        }
    }

    public int GetCurrentIndent()
    {
        var written = this.Written;

        var lastNewLine = written.LastIndexOf(_newline);
        if (lastNewLine < 0)
        {
            goto FAIL;
        }

        var postNewLine = written.Slice(lastNewLine + _newline.Length);
        for (var i = postNewLine.Length - 1; i >= 0; i--)
        {
            if (postNewLine[i] != ' ')
            {
                goto FAIL;
            }
        }
        var ic = postNewLine.Length / 4;
        return ic;


        FAIL:
        return _indentCount;
    }
#endregion

    public void NewLine()
    {
        Write(_newline);
        for (var i = 0; i < _indentCount; i++)
        {
            Write("    "); // 4 spaces
        }
    }






    public void Dispose()
    {
        char[] toReturn = _charArray;
        _charArray = [];
        _position = 0;
        ReturnCharArray(toReturn);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() => new string(_charArray, 0, _position);

    public string ToStringAndDispose()
    {
        string str = this.ToString();
        this.Dispose();
        return str;
    }
}