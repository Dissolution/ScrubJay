using System.Reflection;
using System.Reflection.Emit;

namespace ScrubJay.Universal;

internal static class InternalExtensions
{
#if NET9_0_OR_GREATER
    extension(DynamicMethod)
    {
        public static DynamicMethod New(string methodName,
            Type? returnType,
            params Type[]? parameterTypes)
        {
            var dynamicMethod = new DynamicMethod(
                name: methodName,
                attributes: MethodAttributes.Public | MethodAttributes.Static,
                callingConvention: CallingConventions.Standard,
                returnType: returnType,
                parameterTypes: parameterTypes,
                m: typeof(InternalExtensions).Module,
                skipVisibility: true);

            return dynamicMethod;
        }
    }

    extension(DynamicMethod dynamicMethod)
    {
        public bool TryCreateDelegate<D>([NotNullWhen(true)] out D? del)
            where D : Delegate
        {
            try
            {
                del = dynamicMethod.CreateDelegate<D>();
                return true;
            }
#pragma warning disable CA1031
            catch
#pragma warning restore CA1031
            {
                del = null;
                return false;
            }
        }
    }

    extension(Type? type)
    {
        private MethodInfo? FindMethod(BindingFlags flags,
            string name,
            Type returnType,
            params Type[] parameterTypes)
        {
            if (type is null)
                return null;

            var methods = type.GetMethods(flags);

            foreach (var method in methods)
            {
                if (method.Name.Equals(
                        name,
                        StringComparison.OrdinalIgnoreCase)
                    && method.ReturnType == returnType)
                {
                    var parameters = method.GetParameters();

                    if (parameters.Length != parameterTypes.Length)
                        continue;

                    bool match = true;

                    for (var p = 0; p < parameters.Length; p++)
                    {
                        if (parameters[p].ParameterType != parameterTypes[p])
                        {
                            match = false;
                            break;
                        }
                    }

                    if (match)
                        return method;
                }
            }

            return null;
        }

        public MethodInfo? FindMethod(string name,
            Type returnType,
            params Type[] parameterTypes)
        {
            if (type is null)
                return null;

            BindingFlags flags;

            // Enums
            if (type.IsEnum)
            {
                // Enum instances use the methods on Enum
                type = typeof(Enum);
            }

            // look for an instance method
            flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            // enum, ref-like, and value types we constrain
            if (type.IsEnum || type.IsByRefLike || type.IsByRefLike || type.IsValueType)
            {
                flags |= BindingFlags.DeclaredOnly;
            }

            return FindMethod(
                type,
                flags,
                name,
                returnType,
                parameterTypes);
        }
    }

    extension(ILGenerator generator)
    {
        public void EmitLoadInstance(Type instanceType)
        {
            // stack types
            if (instanceType.IsEnum || instanceType.IsByRef || instanceType.IsByRefLike || instanceType.IsValueType)
            {
                // load a ref to this value
                generator.Emit(
                    OpCodes.Ldarga_S,
                    0);
            }
            // heap types
            else
            {
                // load the value directly
                generator.Emit(OpCodes.Ldarg_0);
            }
        }

        public void EmitCallMethod(Type instanceType,
            MethodInfo method)
        {
            // enums + byref likes we can use Constrained
            if (instanceType.IsByRef || instanceType.IsEnum || instanceType.IsByRefLike)
            {
                generator.Emit(
                    OpCodes.Constrained,
                    instanceType);

                generator.Emit(
                    OpCodes.Callvirt,
                    method);
            }
            // value types we just call
            else
                if (instanceType.IsValueType)
                {
                    generator.Emit(
                        OpCodes.Call,
                        method);
                }
                // class types we have to callvirt
                else
                {
                    generator.Emit(
                        OpCodes.Callvirt,
                        method);
                }
        }
    }
#endif
}