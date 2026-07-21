namespace ScrubJay.Reflection.IL;

[PublicAPI]
public sealed record class InstructionLine(ILOffset Offset, Instruction Instruction);