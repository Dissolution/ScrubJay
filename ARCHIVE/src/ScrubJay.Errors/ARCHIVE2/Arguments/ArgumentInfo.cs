//namespace ScrubJay.Errors.Arguments;
//
//[PublicAPI]
//[StructLayout(LayoutKind.Auto)]
//public readonly record struct ArgumentInfo
//{
//    private const string NAMEOF_NULL = "null";
//
//    public static ArgumentInfo Null() => new(typeof(object), null, NAMEOF_NULL);
//
//    public static ArgumentInfo Null(string? argumentName) => new(typeof(object), argumentName, NAMEOF_NULL);
//
//    public static ArgumentInfo Create(Type? type, string? name, string? toString)
//    {
//        return new ArgumentInfo(type ?? typeof(object), name, toString ?? NAMEOF_NULL);
//    }
//
//    public static ArgumentInfo Capture(
//        object? argument,
//        [CallerArgumentExpression(nameof(argument))]
//        string? argumentName = null)
//    {
//        return new ArgumentInfo(
//            type: argument?.GetType() ?? typeof(object),
//            name: argumentName,
//            valueString: argument?.ToString() ?? NAMEOF_NULL);
//    }
//
//    public static ArgumentInfo Capture<T>(
//        in T? argument,
//        [CallerArgumentExpression(nameof(argument))]
//        string? argumentName = null)
//#if NET9_0_OR_GREATER
//        where T : allows ref struct
//#endif
//    {
//        return new ArgumentInfo(
//            type: Any.GetType<T>(in argument),
//            name: argumentName,
//            valueString: Any.ToStringOr<T>(in argument, NAMEOF_NULL));
//    }
//
//    public readonly Type Type;
//    public readonly string? Name;
//    public readonly string ValueString;
//
//    internal ArgumentInfo(Type type, string? name, string valueString)
//    {
//        Type = type;
//        Name = name;
//        ValueString = valueString;
//    }
//
//    public void Deconstruct(out Type type, out string? name, out string valueString)
//    {
//        type = Type;
//        name = Name;
//        valueString = ValueString;
//    }
//
//    public override string ToString()
//    {
//        return $"\"{Name}\": {Type} = {ValueString}";
//    }
//}