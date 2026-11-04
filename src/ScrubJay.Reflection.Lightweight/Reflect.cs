using ScrubJay.Reflection.Lightweight.Extensions;

namespace ScrubJay.Reflection.Lightweight;

[PublicAPI]
public static class Reflect
{
    internal const BindingFlags METHOD_SEARCH_FLAGS = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;

    private static MethodInfo? FindDeclaredInstanceMethod(
        Type instanceType,
        string methodName,
        Type returnType,
        Type[] parameterTypes)
    {
        return instanceType
            .GetMethods(METHOD_SEARCH_FLAGS)
            .Where(method => string.Equals(method.Name, methodName, StringComparison.Ordinal))
            .Where(method => method.ReturnType == returnType)
            .Where(method => method.GetParameterTypes().SequenceEqual(parameterTypes))
            .OneOrDefault();
    }

    internal static MethodInfo? FindInstanceMethod(
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
}