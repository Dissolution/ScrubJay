// ReSharper disable IdentifierTypo

namespace ScrubJay.Reflection.Lightweight;

[PublicAPI]
public static class ILGeneratorExtensions
{
    extension(ILGenerator generator)
    {
        public ILGenerator Ldarg(int index)
        {
            generator.Emit(OpCodes.Ldarg, index);
            return generator;
        }

        public ILGenerator Ldfld(FieldInfo field)
        {
            generator.Emit(OpCodes.Ldfld, field);
            return generator;
        }
        
        public ILGenerator Ldflda(FieldInfo field)
        {
            generator.Emit(OpCodes.Ldflda, field);
            return generator;
        }
        
        public ILGenerator Ldobj(Type type)
        {
            generator.Emit(OpCodes.Ldobj, type);
            return generator;
        }
        
        public ILGenerator Constrained(Type type)
        {
            generator.Emit(OpCodes.Constrained, type);
            return generator;
        }

        public ILGenerator Call(MethodInfo method)
        {
            generator.Emit(OpCodes.Call, method);
            return generator;
        }
        
        public ILGenerator Callvirt(MethodInfo method)
        {
            generator.Emit(OpCodes.Callvirt, method);
            return generator;
        }

        public void Ret()
        {
            generator.Emit(OpCodes.Ret);
        }
    }
}