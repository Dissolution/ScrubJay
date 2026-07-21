namespace ScrubJay.Reflection.IL;

[PublicAPI]
public class InstructionStream : IReadOnlyCollection<InstructionLine>
{
    private readonly List<InstructionLine> _instructionLines;

    public int Count => _instructionLines.Count;

    public InstructionStream()
    {
        _instructionLines = new(capacity: 32);
    }

    public bool TryFind(ILOffset offset, [NotNullWhen(true)] out InstructionLine? line)
    {
        foreach (var instructionLine in _instructionLines)
        {
            int c = instructionLine.Offset.CompareTo(offset);
            
            if (c == 0)
            {
                line = instructionLine;
                return true;
            }
            
            if (c > 0)
                break;
        }
        
        line = null;
        return false;
    }

    IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();

    IEnumerator<InstructionLine> IEnumerable<InstructionLine>.GetEnumerator() => throw new NotImplementedException();
}