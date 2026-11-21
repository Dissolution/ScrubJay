using System.Reflection;
using System.Reflection.Emit;
using ScrubJay.Reflection.Extensions;

namespace ScrubJay.Reflection;

[PublicAPI]
public static partial class Reflect
{
    private const BindingFlags PID_FLAGS = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;

    private static MethodInfo? FindDeclaredInstanceMethod(
        Type instanceType,
        string methodName,
        Type returnType,
        Type[] parameterTypes)
    {
        var methods = instanceType
            .GetMethods(PID_FLAGS)
            .Where(method => string.Equals(method.Name, methodName, StringComparison.Ordinal))
            .Where(method => method.ReturnType == returnType)
            .Where(method => method.GetParameterTypes().SequenceEqual(parameterTypes))
            .ToList();
        if (methods.Count == 1)
            return methods[0];

        return null;
    }

    public static MethodInfo? FindCallableInstanceMethod(
        Type instanceType,
        string methodName,
        Type returnType,
        Type[] parameterTypes)
    {
        // does the type directly declare it?
        var method = FindDeclaredInstanceMethod(instanceType, methodName, returnType, parameterTypes);
        if (method is not null)
            return method;

        // if the instance type is a ref struct, we can search no further
        if (instanceType.IsByRefLike)
            return null;

        // scan all base types
        var searchType = instanceType.BaseType;
        while (searchType is not null)
        {
            method = FindDeclaredInstanceMethod(searchType, methodName, returnType, parameterTypes);
            if (method is not null)
                return method;
            searchType = searchType.BaseType;
        }

        // scan all interfaces
        var interfaces = instanceType.GetInterfaces();
        foreach (var interfaceType in interfaces)
        {
            method = FindDeclaredInstanceMethod(interfaceType, methodName, returnType, parameterTypes);
            if (method is not null)
                return method;
        }

        // not found
        return null;
    }

    public static DynamicMethod CreateDynamicMethod<D>(string methodName)
        where D : Delegate
    {
        var invokeMethod = typeof(D).GetMethod("Invoke", PID_FLAGS)!;

        var dynamicMethod = new DynamicMethod(
            name: methodName,
            attributes: MethodAttributes.Public | MethodAttributes.Static,
            callingConvention: CallingConventions.Standard,
            returnType: invokeMethod.ReturnType,
            parameterTypes: Array.ConvertAll(invokeMethod.GetParameters(), static p => p.ParameterType),
            m: typeof(Reflect).Module,
            skipVisibility: true);

        return dynamicMethod;
    }
}