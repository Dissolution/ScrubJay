using ScrubJay.Polyfills.Comparison;
#pragma warning disable CS0660, CS0661, CS0659

namespace ScrubJay.Reflection.IL;

[PublicAPI]
public abstract class Instruction :
#if NET7_0_OR_GREATER
    IEqualityOperators<Instruction, Instruction, bool>,
#endif
    IEquatable<Instruction>,
#if NET6_0_OR_GREATER
    ISpanFormattable,
#endif
    IFormattable
{
    public static bool operator ==(Instruction? left, Instruction? right) => Relate.Equate(left, right);
    public static bool operator !=(Instruction? left, Instruction? right) => Relate.Equate(left, right);

    public required OpCode OpCode { get; init; }

    public OpCodeType OpCodeType => OpCode.OpCodeType;

    public OperandType OperandType => OpCode.OperandType;

    public abstract int Size { get; }

    protected Instruction()
    {

    }

    [SetsRequiredMembers]
    protected Instruction(OpCode opcode)
    {
        this.OpCode = opcode;
    }

    public sealed override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is Instruction instruction)
            return Equals(instruction);
        return false;
    }

    public abstract bool Equals(Instruction? other);

    public abstract bool TryFormat(Span<char> destination, out int charsWritten, text format = default, IFormatProvider? provider = null);

    public abstract string ToString(string? format, IFormatProvider? provider = null);
}