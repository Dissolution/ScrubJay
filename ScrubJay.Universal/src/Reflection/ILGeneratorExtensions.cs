using System.Reflection;
using System.Reflection.Emit;

namespace ScrubJay.Universal.Reflection;

[PublicAPI]
public static class ILGeneratorExtensions
{
    extension(ILGenerator generator)
    {
        public void Emit(OpCode opcode, object? arg)
        {
            if (arg is null)
            {
                generator.Emit(opcode);
            }
            else if (arg is byte u8)
            {
                generator.Emit(opcode, u8);
            }
            else if (arg is short i16)
            {
                generator.Emit(opcode, i16);
            }
            else if (arg is long i64)
            {
                generator.Emit(opcode, i64);
            }
            else if (arg is float f32)
            {
                generator.Emit(opcode, f32);
            }
            else if (arg is double f64)
            {
                generator.Emit(opcode, f64);
            }
            else if (arg is int i32)
            {
                generator.Emit(opcode, i32);
            }
            else if (arg is MethodInfo method)
            {
                generator.Emit(opcode, method);
            }
            else if (arg is SignatureHelper signature)
            {
                generator.Emit(opcode, signature);
            }
            else if (arg is ConstructorInfo ctor)
            {
                generator.Emit(opcode, ctor);
            }
            else if (arg is Type type)
            {
                generator.Emit(opcode, type);
            }
            else if (arg is Label label)
            {
                generator.Emit(opcode, label);
            }
            else if (arg is Label[] labels)
            {
                generator.Emit(opcode, labels);
            }
            else if (arg is FieldInfo field)
            {
                generator.Emit(opcode, field);
            }
            else if (arg is string str)
            {
                generator.Emit(opcode, str);
            }
            else if (arg is LocalBuilder local)
            {
                generator.Emit(opcode, local);
            }
            else
            {
                throw new ArgumentException($"Cannot emit a {arg.GetType()} argument", nameof(arg));
            }
        }

        public void Emit((OpCode OpCode, object? Arg) tuple)
            => generator.Emit(tuple.OpCode, tuple.Arg);
    }
}