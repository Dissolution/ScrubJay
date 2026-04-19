namespace ScrubJay.Text.Building;

public partial class TextBuilder
{
    protected static readonly string _newLine = Environment.NewLine;
    protected static readonly int _newLineLength = _newLine.Length;
    
    public TextBuilder NewLine()
    {
        Write(_newLine);
        return this;
    }

    public TextBuilder NewLines(int count)
    {
        if (count > 0)
        {
            if (_newLineLength == 1)
            {
                MaybeGrowBy(count);
                _chars.AsSpan(_position, count).Fill(_newLine[0]);
            }
            else
            {
                for (var i = 0; i < count; i++)
                {
                    Write(_newLine);
                }
            }
            _position += count;
        }
        return this;
    }
}