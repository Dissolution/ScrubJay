using System.Text;
using ScrubJay.Text.Building;

namespace ScrubJay.Validation;

[PublicAPI]
public static partial class Ex
{
    public static DemandException Demand<A, T>(A argument, InterpolatedTextHandler info)
        where A : struct, IArgument<T>
#if NET9_0_OR_GREATER
        , allows ref struct
        where T : allows ref struct
#endif
    {
        var builder = TextBuilder.New
            .Append("Argument `")
            .AppendArgument(argument)
            .Append("` ");
        if (info.Length == 0)
        {
            builder.Append("was invalid");
        }
        else
        {
            builder.Append(info.ToStringAndDispose());
        }

        string message = builder.ReturnAndGetString();
        return new DemandException(Arg.New(argument), message);
    }
    
    public static IndexOutOfRangeException Index(
        int index,
        int available,
        string? info = null,
        [CallerArgumentExpression(nameof(index))]
        string? indexName = null)
    {
        var arg = Argument.New(index, indexName);
        var builder = StringBuilder.Rent()
            .Append("Index `")
            .AppendArgument(arg)
            .Append('`');
        if (info is null)
        {
            builder.Append($" was not in [0..{available})");
        }
        else
        {
            builder.Append(" was invalid: ").Append(info);
        }

        string message = builder.ReturnAndGetString();
        return new IndexOutOfRangeException(message);
    }

    public static ArgumentNullException ArgNull<T>(
        T? value,
        string? info = null,
        [CallerArgumentExpression(nameof(value))]
        string? valueName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        var arg = Argument.New(value, valueName);
        var builder = StringBuilder.Rent()
            .Append("Argument `")
            .AppendArgument(arg)
            .Append("` was null");
        if (info is not null)
        {
            builder.Append(": ").Append(info);
        }

        string message = builder.ReturnAndGetString();
        return new ArgumentNullException(valueName, message);
    }
}