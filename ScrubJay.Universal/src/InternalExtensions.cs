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
        
        public static DynamicMethod New<D>(string methodName)
            where D : Delegate
        {
            var invokeMethod = typeof(D).GetMethod("Invoke", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            if (invokeMethod is null)
            {
                Debugger.Break();
                throw new InvalidOperationException();
            }

            var dynamicMethod = new DynamicMethod(
                name: methodName,
                attributes: MethodAttributes.Public | MethodAttributes.Static,
                callingConvention: CallingConventions.Standard,
                returnType: invokeMethod.ReturnType,
                parameterTypes: Array.ConvertAll(invokeMethod.GetParameters(), static p => p.ParameterType),
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
        internal MethodInfo? FindBestMethod(string name, Type returnType, params Type[] parameterTypes)
        {
            Func<MethodInfo, bool> isMatchingMethod = m =>
            {
                if (m.Name != name || !m.ReturnType.IsAssignableTo(returnType))
                    return false;
                var mp = m.GetParameters();
                if (mp.Length != parameterTypes.Length)
                    return false;
                for (var i = 0; i < mp.Length; i++)
                {
                    if (!mp[i].ParameterType.IsAssignableFrom(parameterTypes[i]))
                        return false;
                }
                return true;
            };
        
            // go through type and all subtypes to find a matching method
        
            while (type is not null)
            {
                MethodInfo? method = type
                    .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(isMatchingMethod)
                    .FirstOrDefault();
                if (method is not null)
                    return method;
                if (type.IsByRefLike)
                    return null; // only methods declared directly on the ref struct can be used
                type = type.BaseType;
            }
            return null;
        }
    }

    extension(ILGenerator generator)
    {
      

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