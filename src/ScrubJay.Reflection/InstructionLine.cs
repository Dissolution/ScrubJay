namespace ScrubJay.Reflection;

[PublicAPI]
public sealed record class InstructionLine(ILOffset Offset, Instruction Instruction);