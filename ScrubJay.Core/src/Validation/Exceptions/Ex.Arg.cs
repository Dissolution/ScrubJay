using ScrubJay.Rendering.Rendition3;
// ReSharper disable MethodOverloadWithOptionalParameter



namespace ScrubJay.Validation;

partial class Ex
{
    internal static TextBuilder AppendArgument(
        this TextBuilder builder,
        string? argumentName,
        Type argumentType)
    {
        return builder
            .Append('"')
            .Append(argumentName ?? "〈?〉")
            .Append("\": ")
            .Append(TypeName.For(argumentType));
    }
    
    internal static TextBuilder AppendArgument(
        this TextBuilder builder,
        string? argumentName,
        Type argumentType,
        string? argumentString)
    {
        return builder
            .Append('"')
            .Append(argumentName ?? "〈?〉")
            .Append("\": ")
            .Append(TypeName.For(argumentType))
            .Append(" = `")
            .IfNotEmpty(argumentString, static (tb, argStr) => tb.Write(argStr), static tb => tb.Write("〈null〉"))
            .Append('`');
    }

    [MustDisposeResource]
    internal static TextBuilder AppendArgument<T>(this TextBuilder builder, string? argumentName, T? argument)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
        => builder.AppendArgument(argumentName, Any.GetType<T>(argument), Any.ToString<T>(argument));

    internal static string GetArgExceptionMessage(string? argumentName, Type argumentType, string? argumentString, string? info)
    {
        return TextBuilder.New
            .Append("Argument ")
            .AppendArgument(argumentName, argumentType, argumentString)
            .Append("was invalid")
            .IfNotEmpty(info, static (tb, n) => tb.Append(": ").Write(n))
            .ToStringAndDispose();
    }

    internal static string GetArgExceptionMessage(string? argumentName, Type argumentType, string? argumentString, ref InterpolatedTextBuilder info)
    {
        return TextBuilder.New
            .Append("Argument ")
            .AppendArgument(argumentName, argumentType, argumentString)
            .Append("was invalid")
            .IfNotEmpty(ref info, static (tb, ref n) => tb.Append(": ").Append(ref n))
            .ToStringAndDispose();
    }

#if NET9_0_OR_GREATER
    internal static string GetArgExceptionMessage<T>(scoped T? argument, string? argumentName, string? info)
        where T : allows ref struct
#else
    internal static string GetArgExceptionMessage<T>(T? argument, string? argumentName, string? info)
#endif
    {
        return TextBuilder.New
            .Append("Argument ")
            .AppendArgument(argumentName, Any.GetType<T>(argument), Any.ToString(argument))
            .Append("was invalid")
            .IfNotEmpty(info, static (tb, n) => tb.Append(": ").Write(n))
            .ToStringAndDispose();
    }

#if NET9_0_OR_GREATER
    internal static string GetArgExceptionMessage<T>(scoped T? argument, string? argumentName, ref InterpolatedTextBuilder info)
        where T : allows ref struct
#else
    internal static string GetArgExceptionMessage<T>(T? argument, string? argumentName, ref InterpolatedTextBuilder info)
#endif
    {
        return TextBuilder.New
            .Append("Argument ")
            .AppendArgument(argumentName, Any.GetType<T>(argument), Any.ToString(argument))
            .Append("was invalid")
            .IfNotEmpty(ref info, static (tb, ref n) => tb.Append(": ").Append(ref n))
            .ToStringAndDispose();
    }

#if NET9_0_OR_GREATER
    public static ArgumentException Arg<T>(scoped T? argument, string? info = null, [CallerArgumentExpression(nameof(argument))] string? argumentName = null)
        where T : allows ref struct
#else
    public static ArgumentException Arg<T>(T? argument, string? info = null, [CallerArgumentExpression(nameof(argument))] string? argumentName = null)
#endif
    {
        string message = GetArgExceptionMessage<T>(argument, argumentName, info);
        return new ArgumentException(message, argumentName);
    }

#if NET9_0_OR_GREATER
    public static ArgumentException Arg<T>(scoped T? argument, ref InterpolatedTextBuilder info, [CallerArgumentExpression(nameof(argument))] string? argumentName = null)
        where T : allows ref struct
#else
    public static ArgumentException Arg<T>(T? argument, ref InterpolatedTextBuilder info, [CallerArgumentExpression(nameof(argument))] string? argumentName = null)
#endif
    {
        string message = GetArgExceptionMessage<T>(argument, argumentName, ref info);
        return new ArgumentException(message, argumentName);
    }



    public static ArgumentException Arg<T>(
        scoped Span<T> argument,
        string? info = default,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
    {
#if NET9_0_OR_GREATER
        return Arg<Span<T>>(argument, info, argumentName);
#else
        string message = GetArgExceptionMessage(argumentName, typeof(Span<T>), argument.ToString(), info);
        return new ArgumentException(message, argumentName);
#endif
    }

    public static ArgumentException Arg<T>(
        scoped Span<T> argument,
        ref InterpolatedTextBuilder info,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
    {
#if NET9_0_OR_GREATER
        return Arg<Span<T>>(argument, ref info, argumentName);
#else
        string message = GetArgExceptionMessage(argumentName, typeof(Span<T>), argument.ToString(), ref info);
        return new ArgumentException(message, argumentName);
#endif
    }

    public static ArgumentException Arg<T>(
        scoped ReadOnlySpan<T> argument,
        string? info = default,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
    {
#if NET9_0_OR_GREATER
        return Arg<ReadOnlySpan<T>>(argument, info, argumentName);
#else
        string message = GetArgExceptionMessage(argumentName, typeof(Span<T>), argument.ToString(), info);
        return new ArgumentException(message, argumentName);
#endif
    }

    public static ArgumentException Arg<T>(
        scoped ReadOnlySpan<T> argument,
        ref InterpolatedTextBuilder info,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
    {
#if NET9_0_OR_GREATER
        return Arg<ReadOnlySpan<T>>(argument, ref info, argumentName);
#else
        string message = GetArgExceptionMessage(argumentName, typeof(Span<T>), argument.ToString(), ref info);
        return new ArgumentException(message, argumentName);
#endif
    }
}