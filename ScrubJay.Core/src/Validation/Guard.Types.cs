namespace ScrubJay.Validation;

partial class Guard
{
    [return: NotNullIfNotNull(nameof(type))]
    public static Type? Implements(Type? type, Type? otherType,
        [CallerArgumentExpression(nameof(type))]
        string? typeName = null)
    {
        if (type.Implements(otherType))
            return type;
        throw Ex.Arg(type, $"does not implement {otherType}", typeName);
    }
    
    [return: NotNullIfNotNull(nameof(type))]
    public static Type? Implements<T>(Type? type,
        [CallerArgumentExpression(nameof(type))]
        string? typeName = null)
    {
        if (type.Implements<T>())
            return type;
        throw Ex.Arg(type, $"does not implement {typeof(T)}", typeName);
    }

    public static Type IsEnum(Type type,
        [CallerArgumentExpression(nameof(type))]
        string? typeName = null)
    {
        if (type.IsEnum)
            return type;
        throw Ex.Arg(type, "is not an enum", typeName);
    }
}