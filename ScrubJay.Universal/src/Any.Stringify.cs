// ReSharper disable MethodOverloadWithOptionalParameter

using System.Reflection;
using System.Reflection.Emit;

namespace ScrubJay.Universal;

partial class Any
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToString<T>(T? value)
    {
        return value?.ToString() ?? string.Empty;
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToString<T>(T? value, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        return MethodCache<T>.ToString(value);
    }
}

partial class MethodCache<T>
{
    private static readonly Lazy<Func<T, string>> _lazyToStringFunc =
        new(CreateToStringFunc, LazyThreadSafetyMode.ExecutionAndPublication);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string FallbackToString(T value) => TypeName.For<T>();

    private static Func<T, string> CreateToStringFunc()
    {
        Type instanceType = typeof(T);
        MethodInfo? toStringMethod = instanceType.FindMethod("ToString", typeof(string), []);

        if (toStringMethod is null)
            return FallbackToString;

        // emit our dynamic method
        var dynamicMethod = DynamicMethod.New($"{TypeName.For<T>()}_ToString", typeof(string), typeof(T));
        var generator = dynamicMethod.GetILGenerator();

        // load instance
        generator.EmitLoadInstance(instanceType);
        // call the method
        generator.EmitCallMethod(instanceType, toStringMethod);
        // return the string on the stack
        generator.Emit(OpCodes.Ret);

        if (!dynamicMethod.TryCreateDelegate<Func<T,string>>(out var func))
        {
            func = FallbackToString;
        }

        return func;
    }
    

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToString(T? value)
    {
        if (value is null)
            return string.Empty;
        return _lazyToStringFunc.Value(value);
    }
}

#endif