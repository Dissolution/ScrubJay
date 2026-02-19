#pragma warning disable CS8620
// ReSharper disable MethodOverloadWithOptionalParameter

using System.Reflection;
using System.Reflection.Emit;

namespace ScrubJay.Universal;

partial class Any
{
    /// <summary>
    /// Determine if a <typeparamref name="T"/> <paramref name="value"/> is equal to an <see cref="object"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="other"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static bool Equals<T>(T? value,
        object? other)
    {
        if (value is null)
            return other is null;

        return value.Equals(other);
    }
}

#if NET9_0_OR_GREATER
partial class Any
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equals<T>(T? value,
        object? other,
        TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        return MethodCache<T>.Equals(
            value,
            other);
    }
}

partial class MethodCache<T>
{
    private static readonly Lazy<Func<T, object?, bool>?> _lazyEqualsObjectFunc = new(
        CreateEqualsObjectFunc,
        LazyThreadSafetyMode.ExecutionAndPublication);

    private static Func<T, object?, bool>? CreateEqualsObjectFunc()
    {
        Type instanceType = typeof(T);

        MethodInfo? equalsMethod = instanceType.FindMethod(
            "Equals",
            typeof(bool),
            typeof(object));

        if (equalsMethod is null)
            return null;

        // emit our dynamic method
        var dynamicMethod = DynamicMethod.New(
            $"{TypeName.For<T>()}_Equals_Object",
            typeof(bool),
            typeof(T),
            typeof(object));

        var generator = dynamicMethod.GetILGenerator();

        // load instance
        generator.EmitLoadInstance(instanceType);
        // load value to compare to
        generator.Emit(OpCodes.Ldarg_1);

        // call the method
        generator.EmitCallMethod(
            instanceType,
            equalsMethod);

        // return the bool on the stack
        generator.Emit(OpCodes.Ret);

        if (!dynamicMethod.TryCreateDelegate<Func<T?, object?, bool>>(out var func))
        {
            return null;
        }

        // try to execute it to see if it will even work
        // Span + ReadOnlySpan throw
        try
        {
            _ = func(
                default,
                new object());
        }
#pragma warning disable
        catch
#pragma warning restore
        {
            return null;
        }

        return func;
    }

    public static bool Equals(T? value,
        object? other)
    {
        if (value is null)
            return other is null;

        var func = _lazyEqualsObjectFunc.Value;

        if (func is not null)
        {
            return func.Invoke(
                value,
                other);
        }

        return other is not null; // only way we have to equate
    }
}

#endif