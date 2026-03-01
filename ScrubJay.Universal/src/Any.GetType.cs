// ReSharper disable MethodOverloadWithOptionalParameter

using System.Reflection;
using System.Reflection.Emit;
#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
namespace ScrubJay.Universal;

partial class Any
{
    /// <summary>
    /// Gets the <see cref="Type"/> of a <typeparamref name="T"/> <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// The <typeparamref name="T"/> value to get the true <see cref="Type"/> of.
    /// </param>
    /// <typeparam name="T">
    /// The generic <see cref="Type"/> of this method invocation, which may be less specific than the true <see cref="Type"/>.
    /// </typeparam>
    /// <returns>
    /// The true <see cref="Type"/> of <paramref name="value"/>, which may be more specific than <c>typeof(T)</c>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Type GetType<T>(ref readonly T? value)
    {
        if (value is not null)
        {
            return value.GetType();
        }

        return typeof(T);
    }
}

#if NET9_0_OR_GREATER
partial class Any
{
    /// <summary>
    /// Gets the <see cref="Type"/> of a <typeparamref name="T"/> <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// The <typeparamref name="T"/> value to get the true <see cref="Type"/> of.
    /// </param>
    /// <typeparam name="T">
    /// The generic <see cref="Type"/> of this method invocation.
    /// </typeparam>
    /// <returns><c>typeof(T)</c></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Type GetType<T>(ref readonly T? value, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        if (value is null)
            return typeof(T);
        return MethodCache<T>.LazyGetType.Value.Invoke(in value);
    }
}

partial class MethodCache<T>
{
    public delegate Type AnyGetType(ref readonly T value);

    public static readonly Lazy<AnyGetType> LazyGetType = new(CreateGetTypeFunc, LazyThreadSafetyMode.ExecutionAndPublication);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Type FallbackGetType(ref readonly T _) => typeof(T);
    
    private static AnyGetType CreateGetTypeFunc()
    {
        Type instanceType = typeof(T);
        MethodInfo? getTypeMethod = FindBestMethod<AnyGetType>(instanceType, nameof(object.GetType));

        if (getTypeMethod is null)
            return FallbackGetType;

        // emit our dynamic method
        var dynamicMethod = DynamicMethod.New<AnyGetType>($"Any_{Type.Render<T>()}_GetType");
        var generator = dynamicMethod.GetILGenerator();
        
        generator.Emit(OpCodes.Ldarg_0);
        generator.Emit(OpCodes.Constrained, instanceType);
        generator.Emit(OpCodes.Callvirt, getTypeMethod);
        generator.Emit(OpCodes.Ret);
        
        if (!dynamicMethod.TryCreateDelegate<AnyGetType>(out var func))
        {
            func = FallbackGetType;
        }

        return func;
    }
}


#endif