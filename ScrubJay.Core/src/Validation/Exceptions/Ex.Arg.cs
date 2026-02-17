using ScrubJay.Rendering.Rendition3;



namespace ScrubJay.Validation;

partial class Ex
{
    internal static string GetArgExceptionMessage<T>(
        T? argument,
        string? argumentName,
        InterpolatedTextBuilder info,
        InterpolatedTextBuilder fallback = default)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        var builder = TextBuilder.New
            .Append("Argument ")
            .AppendArgument(argument, argumentName)
            .Append(' ');

        if (info.Length > 0)
        {
            builder.Append(ref info);
        }
        else if (fallback.Length > 0)
        {
            builder.Append(ref fallback);
        }
        else
        {
            builder.Append("was invalid");
        }

        return builder.ToStringAndDispose();
    }

    public static ArgumentException Arg<T>(
        T? argument,
        InterpolatedTextBuilder info = default,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        string message = GetArgExceptionMessage<T>(argument, argumentName, info);
        return new ArgumentException(message);
    }


    public static ArgumentException Arg(
        object? argument,
        InterpolatedTextBuilder info = default,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
    {
        string message = GetArgExceptionMessage(argument?.GetType(), argument?.ToString(), argumentName, info);
        return new ArgumentException(message);
    }

    public static ArgumentException Arg(
        Type? argumentType,
        string? argumentString,
        string? argumentName,
        InterpolatedTextBuilder info = default)
    {
        var message = GetArgExceptionMessage(argumentType, argumentString, argumentName, info);
        return new ArgumentException(message);
    }


    public static ArgumentException Arg<T>(
        scoped Span<T> argument,
        InterpolatedTextBuilder info = default,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
    {
#if NET9_0_OR_GREATER
        return Arg<Span<T>>(argument, info, argumentName);
#else
        return Arg(typeof(Span<T>), argument.Render(), argumentName, info);
#endif
    }

    public static ArgumentException Arg<T>(
        scoped ReadOnlySpan<T> argument,
        InterpolatedTextBuilder info = default,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
    {
#if NET9_0_OR_GREATER
        return Arg<ReadOnlySpan<T>>(argument, info, argumentName);
#else
        return Arg(typeof(Span<T>), argument.Render(), argumentName, info);
#endif
    }
}