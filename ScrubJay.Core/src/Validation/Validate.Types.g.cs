#nullable enable

namespace ScrubJay.Validation;

partial class Validate
{
    [return: NotNullIfNotNull(nameof(type))]
    public static Result<Type?> Implements(Type? type, Type? otherType,
        [CallerArgumentExpression(nameof(type))]
        string? typeName = null)
    {
        if (type.Implements(otherType))
            return type;
        return Ex.Arg(type, $"does not implement {otherType}", typeName);
    }
    
    [return: NotNullIfNotNull(nameof(type))]
    public static Result<Type?> Implements<T>(Type? type,
        [CallerArgumentExpression(nameof(type))]
        string? typeName = null)
    {
        if (type.Implements<T>())
            return type;
        return Ex.Arg(type, $"does not implement {typeof(T)}", typeName);
    }

    public static Result<Type> IsEnum(Type type,
        [CallerArgumentExpression(nameof(type))]
        string? typeName = null)
    {
        if (type.IsEnum)
            return type;
        return Ex.Arg(type, "is not an enum", typeName);
    }
}
