using System.Reflection;
using System.Reflection.Emit;
using ScrubJay.Universal.Extensions;

namespace ScrubJay.Universal;

/// <summary>
/// A static utility class for working with <b>any</b> generic value,<br/>
/// including ones with the <c>allows ref struct</c> anti-constraint.
/// </summary>
/// <see href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/ref-struct"/>
[PublicAPI]
public static partial class Any
{
    internal static DynamicMethod CreateDynamicMethod<D>(string methodName)
        where D : Delegate
    {
        var invokeMethod = typeof(D)
            .GetMethod("Invoke", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

        if (invokeMethod is null)
            throw new InvalidOperationException("Could not find Delegate Invoke Method");

        var dynamicMethod = new DynamicMethod(
            name: methodName,
            attributes: MethodAttributes.Public | MethodAttributes.Static,
            callingConvention: CallingConventions.Standard,
            returnType: invokeMethod.ReturnType,
            parameterTypes: Array.ConvertAll(invokeMethod.GetParameters(), static p => p.ParameterType),
            m: typeof(Any).Module,
            skipVisibility: true);

        return dynamicMethod;
    }

    internal static bool TryCreateDelegate<D>(
        this DynamicMethod dynamicMethod,
        [NotNullWhen(true)] out D? del)
        where D : Delegate
    {
        try
        {
            del = dynamicMethod.CreateDelegate(typeof(D)) as D;
            return del is not null;
        }
        catch
        {
            del = null;
            return false;
        }
    }

    internal static bool TryGenerateDelegate<D>(
        string methodName,
        Action<ILGenerator> generateBody,
        [NotNullWhen(true)] out D? del)
        where D : Delegate
    {
        var dynamicMethod = CreateDynamicMethod<D>(methodName);
        generateBody(dynamicMethod.GetILGenerator());
        return dynamicMethod.TryCreateDelegate<D>(out del);
    }



    private static Func<MethodInfo, bool> GetMatchPredicate(string? name, Type? returnType, Type?[]? parameterTypes)
    {
        Func<MethodInfo, bool>? predicate = null;

        if (!string.IsNullOrEmpty(name))
        {
            predicate &= method => string.Equals(method.Name, name, StringComparison.Ordinal);
        }

        if (returnType is not null)
        {
            predicate &= method => method.ReturnType.IsAssignableTo(returnType);
        }

        if (parameterTypes is not null)
        {
            int parameterCount = parameterTypes.Length;
            predicate &= method =>
            {
                var mp = method.GetParameters();
                if (mp.Length != parameterCount)
                    return false;
                for (var i = 0; i < mp.Length; i++)
                {
                    var pt = parameterTypes[i];
                    if (pt is null)
                        continue;
                    if (!mp[i].ParameterType.IsAssignableFrom(pt))
                        return false;
                }
                return true;
            };
        }

        return predicate ?? Predicate<MethodInfo>.True;
    }

    private static IEnumerable<MethodInfo> FindMatchingMethods(
        this Type? type,
        BindingFlags bindingFlags,
        string? name,
        Type? returnType,
        params Type?[]? parameterTypes)
    {
        var match = GetMatchPredicate(name, returnType, parameterTypes);

        // go through type and all subtypes to find a matching method
        bindingFlags |= BindingFlags.DeclaredOnly;

        while (type is not null)
        {
            foreach (var method in type
                .GetMethods(bindingFlags)
                .Where(match))
            {
                yield return method;
            }
            if (type.IsByRefLike)
                break;
            type = type.BaseType;
        }
    }

    internal static IEnumerable<MethodInfo> FindMatchingInstanceMethods(
        this Type? type,
        string? name,
        Type? returnType,
        params Type?[]? parameterTypes)
        => FindMatchingMethods(type, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, name, returnType, parameterTypes);

    internal static IEnumerable<MethodInfo> FindMatchingStaticMethods(
        this Type? type,
        string? name,
        Type? returnType,
        params Type?[]? parameterTypes)
        => FindMatchingMethods(type, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, name, returnType, parameterTypes);

#pragma warning disable IDE0060
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ReadOnlySpan<byte> GetReferenceBytes<T>(scoped ref readonly T value)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        Emit.Ldarg(nameof(value));
        Emit.Conv_U();
        Emit.Sizeof<T>();
        Emit.Newobj(MethodRef.Constructor(typeof(ReadOnlySpan<byte>), [typeof(void*), typeof(int)]));
        Emit.Ret();
        throw Unreachable();
    }
}