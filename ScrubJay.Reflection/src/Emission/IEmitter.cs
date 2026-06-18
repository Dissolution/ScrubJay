using ScrubJay.Errors;
using ScrubJay.Reflection.Decompilation;

namespace ScrubJay.Reflection.Emission;

[PublicAPI]
public interface IEmitter
{

}

[PublicAPI]
public static class EmissionExtensions
{
    extension(ILGenerator generator)
    {
        public void Thing()
        {

        }
    }

    extension(OperandType operandType)
    {
        public Type[] GetArgumentTypes()
        {
            switch (operandType)
            {
                case OperandType.InlineBrTarget:
                    return [typeof(int), typeof(ILOffset)];
                case OperandType.InlineField:
                    return [typeof(MetadataToken), typeof(FieldInfo)];
                case OperandType.InlineI:
                    return [typeof(int)];
                case OperandType.InlineI8:
                    return [typeof(long)];
                case OperandType.InlineMethod:
                    return [typeof(MetadataToken), typeof(MethodInfo)];
                case OperandType.InlineR:
                    return [typeof(double)];
                case OperandType.InlineSig:
                    return [typeof(MetadataToken)];
                case OperandType.InlineString:
                    return [typeof(MetadataToken), typeof(string)];
                case OperandType.InlineSwitch:
                    return [typeof(ILOffset[])];
                case OperandType.InlineTok:
                    return [typeof(MetadataToken), typeof(FieldInfo), typeof(MethodInfo), typeof(Type)];
                case OperandType.InlineType:
                    return [typeof(MetadataToken), typeof(Type)];
                case OperandType.InlineVar:
                    return [typeof(short)];
                case OperandType.ShortInlineBrTarget:
                    return [typeof(sbyte)];
                case OperandType.ShortInlineI:
                    return [typeof(sbyte)];
                case OperandType.ShortInlineR:
                    return [typeof(float)];
                case OperandType.ShortInlineVar:
                    return [typeof(sbyte)];
                case OperandType.InlineNone:
#pragma warning disable CS0618 // Type or member is obsolete
                case OperandType.InlinePhi:
                default:
                    return [];
            }
        }

        public int? Size
        {
            get
            {
                switch (operandType)
                {
                    case OperandType.InlineSwitch:
                    {
                        return null;
                    }
                    case OperandType.InlineI8:
                    case OperandType.InlineR:
                    {
                        return 8;
                    }
                    case OperandType.InlineBrTarget:
                    case OperandType.InlineField:
                    case OperandType.InlineI:
                    case OperandType.InlineMethod:
                    case OperandType.InlineSig:
                    case OperandType.InlineString:
                    case OperandType.InlineTok:
                    case OperandType.InlineType:
                    case OperandType.ShortInlineR:
                    {
                        return 4;
                    }
                    case OperandType.InlineVar:
                    {
                        return 2;
                    }
                    case OperandType.ShortInlineBrTarget:
                    case OperandType.ShortInlineI:
                    case OperandType.ShortInlineVar:
                    {
                        return 1;
                    }
                    case OperandType.InlinePhi:
                    case OperandType.InlineNone:
                    default:
                    {
                        return 0;
                    }
                }
            }
        }
    }
}

[PublicAPI]
public class Instructions : IReadOnlyCollection<InstructionLine>
{
    public int Count { get; }

    public Option<Instruction> GetAt(ILOffset offset)
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<InstructionLine> GetEnumerator() => throw new NotImplementedException();
}

[PublicAPI]
public sealed record class InstructionLine(ILOffset Offset, Instruction Instruction);

[PublicAPI]
public abstract record class Instruction
{
    /// <summary>
    /// Gets the total size of this instruction (in bytes).
    /// </summary>
    public abstract int Size { get; }
}

[PublicAPI]
public record class OpCodeInstruction : Instruction
{
    public OpCode OpCode { get; }

    public string? Name => OpCode.Name;

    public OperandType OperandType => OpCode.OperandType;

    public OpCodeType OpCodeType => OpCode.OpCodeType;

    public override int Size => OpCode.Size;

    public OpCodeInstruction(OpCode opCode)
    {
        OpCode = opCode;
    }
}

[PublicAPI]
public abstract record class OpCodeValueInstruction : OpCodeInstruction
{
    public object? Argument { get; }

    public override int Size
    {
        get
        {
            var size = OpCode.Size;
            if (OperandType == OperandType.InlineSwitch)
            {
                if (Argument is not Array array)
                    throw new InvalidOperationException();
                return size + (1 + array.Length) * 4;
            }
            else
            {
                Debug.Assert(OperandType.Size is not null);
                return size + OperandType.Size.GetValueOrDefault();
            }
        }
    }

    protected OpCodeValueInstruction(OpCode opCode, object? argument)
        : base(opCode)
    {
        if (OperandType == OperandType.InlineNone)
            throw Ex.Arg(opCode, $"OpCode.{opCode.Name} does not require an argument");
        Argument = argument;
    }
}

[PublicAPI]
public record class OpCodeValueInstruction<T> : OpCodeValueInstruction
{
    public T Value { get; }

    public OpCodeValueInstruction(OpCode opCode, T value)
        : base(opCode, (object?)value)
    {
        Value = value;
    }
}