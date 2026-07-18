using ScrubJay.Errors;
using ScrubJay.Polyfills.Text;

namespace ScrubJay.Reflection;

public abstract class ArgInstruction<A> : Instruction
{
    public required A Argument { get; init; }
    
    [SetsRequiredMembers]
    public ArgInstruction(OpCode opcode, A argument) : base(opcode)
    {
        if (opcode.OperandType == OperandType.InlineNone)
            throw Ex.Arg(opcode, "must not have OperandType.InlineNone");
        this.Argument = argument;
    }

    public sealed override bool Equals(Instruction? other) => other is ArgInstruction<A> instruction && Equals(instruction);

    public abstract bool Equals(ArgInstruction<A>? other);
    
    public override int GetHashCode() => HashCode.Combine(OpCode, Argument);

    public override bool TryFormat(Span<char> destination, out int charsWritten, text format = default, IFormatProvider? provider = null)
    {
        return new TryFormatter(destination)
        {
            OpCode,
            ": ",
            {Argument, format, provider},
        }.Wrote(out charsWritten);
    }

    public override string ToString(string? format, IFormatProvider? provider = null)
    {
        using var builder = new InterpolatedText(2, 2);
        builder.Write(OpCode);
        builder.Write(": ");
        builder.Format(Argument, format, provider);
        return builder.ToString();
    }

    public override string ToString() => $"{OpCode}: {Argument}";
}