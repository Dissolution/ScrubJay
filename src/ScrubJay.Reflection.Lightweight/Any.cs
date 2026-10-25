using ScrubJay.Reflection.Lightweight.Extensions;

namespace ScrubJay.Reflection.Lightweight;

[PublicAPI]
public static class Any
{
    [return: NotNullIfNotNull(nameof(value))]
    public static string? ToString<T>(ref readonly T? value)
        where T : allows ref struct
    {
        return AnyToStringCache<T>.Invoke(in value);
    }

    public static Type GetType<I>(ref readonly I? instance)
        where I : allows ref struct
    {
        
    }
}

[return: NotNullIfNotNull(nameof(value))]
public delegate string? AnyToString<T>(ref readonly T? value)
    where T : allows ref struct;

internal static class AnyToStringCache<T>
    where T : allows ref struct
{
    public static readonly AnyToString<T> Invoke = Create();

    private static AnyToString<T> Create()
    {
        var toStringMethod = DynamicMethod.Create($"Any_{typeof(T)}_ToString", typeof(string), [typeof(T).MakeByRefType()]);

        throw new NotImplementedException();
    }
}