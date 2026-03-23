#pragma warning disable CS8620
// ReSharper disable MethodOverloadWithOptionalParameter

using System.Reflection;
using System.Reflection.Emit;

namespace ScrubJay.Universal;

partial class Any
{
    /// <summary>
    /// Determines whether a <typeparamref name="T"/> <paramref name="value"/> is equal to an <see cref="object"/>.
    /// </summary>
    /// <param name="value">
    /// The <typeparamref name="T"/> value to compare.
    /// </param>
    /// <param name="other">
    /// The <see cref="object"/> to compare.
    /// </param>
    /// <typeparam name="T">
    /// The <see cref="Type"/> of <paramref name="value"/> to compare to the <see cref="object"/>.
    /// </typeparam>
    /// <returns>
    /// <see langword="true"/> if the <paramref name="value"/> and <see cref="object"/> are equal; otherwise <see langword="false"/>.
    /// </returns>
    public static bool Equals<T>(T? value, object? other)
    {
        if (value is null)
            return other is null;
        return value.Equals(other);
    }
}

#if NET9_0_OR_GREATER
partial class Any
{
    /// <summary>
    /// Determines whether a <typeparamref name="T"/> <paramref name="value"/> is equal to an <see cref="object"/>.
    /// </summary>
    /// <param name="value">
    /// The <typeparamref name="T"/> value to compare.
    /// </param>
    /// <param name="other">
    /// The <see cref="object"/> to compare.
    /// </param>
    /// <param name="_"></param>
    /// <typeparam name="T">
    /// The <see cref="Type"/> of <paramref name="value"/> to compare to the <see cref="object"/>.
    /// </typeparam>
    /// <returns>
    /// <see langword="true"/> if the <paramref name="value"/> and <see cref="object"/> are equal; otherwise <see langword="false"/>.
    /// </returns>
    public static bool Equals<T>(T? value,
        object? other,
        TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        if (value is null)
            return other is null;
        return MethodCache<T>.LazyEqualsObject.Value.Invoke(value, other);
    }
}

partial class MethodCache<T>
{
    public static readonly Lazy<Func<T, object?, bool>> LazyEqualsObject = new(
        CreateEqualsObjectFunc,
        LazyThreadSafetyMode.ExecutionAndPublication);

    private static bool EqualsObjectFallback(T value, object? obj) => false;
    
    private static Func<T, object?, bool> CreateEqualsObjectFunc()
    {
        Type instanceType = typeof(T);

        MethodInfo? equalsMethod = instanceType.FindBestMethod("Equals", typeof(bool), typeof(object));

        if (equalsMethod is null)
            return EqualsObjectFallback;

        // emit our dynamic method
        var dynamicMethod = DynamicMethod.New(
            $"Equals_{Type.Render<T>()}_Object",
            typeof(bool),
            typeof(T),
            typeof(object));

        var generator = dynamicMethod.GetILGenerator();

        // load instance
        EmitLoadInstance(generator, instanceType);
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
            return EqualsObjectFallback;
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
            return EqualsObjectFallback;
        }

        return func;
    }
}

#endif