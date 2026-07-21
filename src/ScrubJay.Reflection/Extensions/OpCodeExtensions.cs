using System.Globalization;
using System.Text.RegularExpressions;

namespace ScrubJay.Reflection;

[PublicAPI]
public static
#if NET7_0_OR_GREATER
    partial
#endif
    class OpCodeExtensions
{
#if NET7_0_OR_GREATER
    [GeneratedRegex(@"(?:ld|st)loc[\.as]*([0123])?", RegexOptions.Compiled)]
    private static partial Regex LocalOpCodeRegex();

    [GeneratedRegex(@"(?:ld|st)arg[\.as]*([0123])?", RegexOptions.Compiled)]
    private static partial Regex ArgumentOpCodeRegex();
#endif

    private static readonly OpCode[] _oneByteOpcodes = new OpCode[0x100];
    private static readonly OpCode[] _twoByteOpcodes = new OpCode[0x1F];
    private static readonly OpCode[] _allOpcodes;

    static OpCodeExtensions()
    {
        var opcodes = typeof(OpCodes)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Where(static field => field.FieldType == typeof(OpCode))
            .SelectWhere(static field => Option.Is<OpCode>(field.GetValue(null)))
            .ToList();

        int maxonelow = int.MinValue;
        int maxtwolow = int.MinValue;

        foreach (var opcode in opcodes)
        {
            var oct = opcode.OpCodeType;
            if (oct is default(OpCodeType) /*or OpCodeType.Nternal*/ or > OpCodeType.Primitive)
            {
                Debugger.Break();
            }

            var size = opcode.Size;
            ushort index = (ushort)opcode.Value;
            byte hi = (byte)(index >> 8);
            byte lo = (byte)(index);
            if (size == 1)
            {
                if (hi > 0 || index > 255)
                    Debugger.Break();
                _oneByteOpcodes[lo] = opcode;
                if (lo > maxonelow)
                    maxonelow = lo;
            }
            else if (size == 2)
            {
                if (hi != 0XFE)
                    Debugger.Break();
                _twoByteOpcodes[lo] = opcode;
                if (lo > maxtwolow)
                    maxtwolow = lo;
            }
            else
            {
                Debugger.Break();
                continue;
            }
        }

        var validOneBytes = _oneByteOpcodes
            .Where(o => o.Name is not null)
            .ToArray();
        var validTwoBytes = _twoByteOpcodes
            .Where(o => o.Name is not null)
            .ToArray();


        _allOpcodes = validOneBytes.Concat(validTwoBytes).ToArray();
    }

    extension(OpCodes)
    {
        public static IReadOnlyList<OpCode> All => _allOpcodes;
    }

    extension(OpCode opcode)
    {
        public bool IsPrefix
        {
            get
            {
                bool isPrefix = opcode.OpCodeType == OpCodeType.Prefix;
                bool check = opcode == OpCodes.Unaligned ||
                    opcode == OpCodes.Readonly ||
                    opcode == OpCodes.Volatile ||
                    opcode == OpCodes.Tailcall;
                if (check != isPrefix)
                    Debugger.Break();
                return isPrefix;
            }
        }

        public bool UsesLocal()
        {
#if NET7_0_OR_GREATER
            var regex = LocalOpCodeRegex();
#else
            var regex = new Regex(@"(?:ld|st)loc[\.as]*([0123])?", RegexOptions.Compiled);
#endif
            var match = regex.Match(opcode.Name!);
            return match.Success;
        }

        public bool UsesLocal(out int localIndex)
        {
#if NET7_0_OR_GREATER
            var regex = LocalOpCodeRegex();
#else
            var regex = new Regex(@"(?:ld|st)loc[\.as]*([0123])?", RegexOptions.Compiled);
#endif
            var match = regex.Match(opcode.Name!);
            if (match.Success)
            {
                int gc = match.Groups.Count;
                if (gc == 1)
                {
                    // we use locals, but aren't sure which one
                    localIndex = -1;
                    return true;
                }
                if (gc == 2)
                {
                    localIndex = int.Parse(match.Groups[1].Value, NumberStyles.None);
                    return true;
                }
                else
                {
                    Debugger.Break();
                }
            }

            // does not use locals
            localIndex = -1;
            return false;
        }

        public bool UsesArgument()
        {
#if NET7_0_OR_GREATER
            var regex = ArgumentOpCodeRegex();
#else
            var regex = new Regex(@"(?:ld|st)arg[\.as]*([0123])?", RegexOptions.Compiled);
#endif
            var match = regex.Match(opcode.Name!);
            return match.Success;
        }

        public bool UsesArgument(out int argIndex)
        {
#if NET7_0_OR_GREATER
            var regex = ArgumentOpCodeRegex();
#else
            var regex = new Regex(@"(?:ld|st)arg[\.as]*([0123])?", RegexOptions.Compiled);
#endif
            var match = regex.Match(opcode.Name!);
            if (match.Success)
            {
                int gc = match.Groups.Count;
                if (gc == 1)
                {
                    // we use an argument, but aren't sure which one
                    argIndex = -1;
                    return true;
                }
                if (gc == 2)
                {
                    argIndex = int.Parse(match.Groups[1].Value, NumberStyles.None);
                    return true;
                }
                else
                {
                    Debugger.Break();
                }
            }

            // does not use an argument
            argIndex = -1;
            return false;
        }
    }
}

// 
//    public static OpCode ReadOpCode(ref SpanReader<byte> reader)
//    {
//        OpCode opCode;
//
//        byte u8 = reader.Take();
//
//        if (u8 != 0xFE)
//        {
//            opCode = OneByteOpCodes[u8];
//            if (string.IsNullOrEmpty(opCode.Name))
//                throw new InvalidOperationException($"Invalid one-byte OpCode for 0x{u8:X}");
//        }
//        else
//        {
//            u8 = reader.Take();
//            opCode = TwoByteOpCodes[u8];
//            if (string.IsNullOrEmpty(opCode.Name))
//                throw new InvalidOperationException($"Invalid two-byte OpCode for 0x{u8:X}");
//        }
//
//        return opCode;
//    }
//
//    public static Result<OpCode> TryReadOpCode(ref SpanReader<byte> reader)
//    {
//        OpCode opCode;
//        byte u8 = reader.Take();
//
//        if (u8 != 0xFE)
//        {
//            opCode = OneByteOpCodes[u8];
//            if (string.IsNullOrEmpty(opCode.Name))
//                return new InvalidOperationException($"Invalid one-byte OpCode for 0x{u8:X}");
//        }
//        else
//        {
//            u8 = reader.Take();
//            opCode = TwoByteOpCodes[u8];
//            if (string.IsNullOrEmpty(opCode.Name))
//                return new InvalidOperationException($"Invalid two-byte OpCode for 0x{u8:X}");
//        }
//
//        return Ok(opCode);
//    }
//}