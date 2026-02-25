#pragma warning disable CS8620
// ReSharper disable MethodOverloadWithOptionalParameter

using System.Reflection;
using System.Reflection.Emit;

namespace ScrubJay.Universal;

partial class Any
{
    /// <summary>
    /// Returns the <see cref="string"/> representation of this <typeparamref name="T"/> <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// The <typeparamref name="T"/> instance to call <see cref="object.ToString"/> on.
    /// </param>
    /// <typeparam name="T">
    /// The <see cref="Type"/> of <paramref name="value"/>.
    /// </typeparam>
    /// <returns>
    /// <c>value?.ToString()</c>
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNullIfNotNull(nameof(value))]
    public static string? ToString<T>(ref readonly T? value)
    {
        return value?.ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToString<T>(scoped ReadOnlySpan<T> span)
    {
        return span.ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToString<T>(scoped Span<T> span)
    {
        return span.ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToString(scoped text text)
    {
        return text.ToString();
    }
}

#if NET9_0_OR_GREATER
partial class Any
{
    /// <summary>
    /// Returns the <see cref="string"/> representation of this <typeparamref name="T"/> <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// The <typeparamref name="T"/> instance to call <see cref="object.ToString"/> on.
    /// </param>
    /// <param name="_">
    /// Ignored <see cref="TypeConstraints"/> to assist in method overloading.
    /// </param>
    /// <typeparam name="T">
    /// The <see cref="Type"/> of <paramref name="value"/>, may be a <c>ref struct</c>.
    /// </typeparam>
    /// <returns>
    /// <c>value?.ToString()</c>
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNullIfNotNull(nameof(value))]
    public static string? ToString<T>(ref readonly T? value, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        if (value is null)
            return null;
        return MethodCache<T>.LazyToString.Value.Invoke(in value);
    }
}

partial class MethodCache<T>
{
    public delegate string AnyToString(ref readonly T value);
    
    public static readonly Lazy<AnyToString> LazyToString =
        new(CreateToStringFunc, LazyThreadSafetyMode.ExecutionAndPublication);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string FallbackToString(ref readonly T _) => $"({Type.Render<T>()})???";

    
    private static MethodInfo? FindBestToStringMethod(Type? type)
    {
        while (type is not null)
        {
            MethodInfo? toStringMethod = GetDeclaredToStringMethod(type);
            if (toStringMethod is not null)
                return toStringMethod;
            type = type.BaseType;
        }
        
        return null;
    }

    private static MethodInfo? GetDeclaredToStringMethod(Type type)
    {
        return type
            .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .Where(IsToStringMethod)
            .FirstOrDefault();
    }
    
    private static bool IsToStringMethod(MethodInfo method)
    {
        return method.Name == nameof(ToString) && method.ReturnType == typeof(string) && method.GetParameters().Length == 0;
    }
    
    private static AnyToString CreateToStringFunc()
    {
        Type instanceType = typeof(T);
        MethodInfo? toStringMethod = FindBestToStringMethod(instanceType);

        if (toStringMethod is null)
            return FallbackToString;

        // emit our dynamic method
        var dynamicMethod = DynamicMethod.New<AnyToString>($"Any_{Type.Render<T>()}_ToString");
        var generator = dynamicMethod.GetILGenerator();


        if (instanceType.IsByRef || instanceType.IsByRefLike)
        {
            // ref struct have to have tostring directly declared
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Constrained, instanceType);
            generator.Emit(OpCodes.Callvirt, toStringMethod);
        }



        if (!dynamicMethod.TryCreateDelegate<AnyToString>(out var func))
        {
            func = FallbackToString;
        }

        return func;
    }
}

#endif