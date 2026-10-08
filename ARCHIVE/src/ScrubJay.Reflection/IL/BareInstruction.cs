using ScrubJay.Errors;

namespace ScrubJay.Reflection.IL;

public sealed class BareInstruction : Instruction
{
    public override int Size => OpCode.Size;

    [SetsRequiredMembers]
    public BareInstruction(OpCode opcode) : base(opcode)
    {
        if (opcode.OperandType != OperandType.InlineNone)
            throw Ex.Arg(opcode, "must have OperandType.InlineNone");
    }

    public override bool Equals(Instruction? other) => other is BareInstruction instruction && Equals(instruction);
    
    public bool Equals(BareInstruction? other) => other is not null && other.OpCode == OpCode;

    public override int GetHashCode() => HashCode.Combine(OpCode);

    public override bool TryFormat(Span<char> destination, out int charsWritten, text format = default, IFormatProvider? provider = null)
    {
        var name = OpCode.Name!;
        if (name.TryCopyTo(destination))
        {
            charsWritten = name.Length;
            return true;
        }
        charsWritten = 0;
        return false;
    }

    public override string ToString(string? format, IFormatProvider? provider = null) => OpCode.Name!;

    public override string ToString() => OpCode.Name!;
}