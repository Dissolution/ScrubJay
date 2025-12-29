using System.Text;
using ScrubJay.Text.Building;

namespace ScrubJay.Validation;

internal static class StringBuilderExtensions
{
    extension(TextBuilder builder)
    {
        public StringBuilder AppendArgument<A>(A argument)
            where A : struct, IArgument
#if NET9_0_OR_GREATER
            , allows ref struct
#endif
        {
            return builder
                .Append(argument.Name)
                .Append(": ")
                .AppendTypeName(argument.Type)
                .Append(" = ")
                .Append(argument.ValueString);
        }

        public StringBuilder AppendArgument<A, T>(A argument)
            where A : struct, IArgument<T>
#if NET9_0_OR_GREATER
            , allows ref struct
            where T : allows ref struct
#endif
        {
            return builder
                .Append(argument.Name)
                .Append(": ")
                .AppendTypeName(argument.Type)
                .Append(" = ")
                .Append(argument.Value);
        }
    }
}