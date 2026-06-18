namespace ScrubJay.Universal.Extensions;

[PublicAPI]
public static class TextExtensions
{
    public static bool IsNullOrEmpty([NotNullWhen(false)] this string? str) => string.IsNullOrEmpty(str);

    extension(scoped text text)
    {
        public bool IsNullOrEmpty() => text.IsEmpty;
    }


    extension(ref DefaultInterpolatedStringHandler handler)
    {
        public void Write(in char ch)
        {
            handler.AppendFormatted(ch.AsSpan());
        }

        public void Write(string? str)
        {
            handler.AppendFormatted(str);
        }

        public void Write(text text)
        {
            handler.AppendFormatted(text);
        }

        public void Write<T>(T? value)
        {
            handler.AppendFormatted<T>(value!);
        }

#if NET9_0_OR_GREATER
        // ReSharper disable once MethodOverloadWithOptionalParameter
        public void Write<T>(T? value, TypeConstraints.AllowsRefStruct<T> _ = default)
            where T : allows ref struct
        {
            handler.AppendFormatted(Any.ToString<T>(in value));
        }
#endif
    }
}