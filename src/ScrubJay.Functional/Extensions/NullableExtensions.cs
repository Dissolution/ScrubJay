namespace ScrubJay.Functional.Extensions;

[PublicAPI]
public static class NullableExtensions
{
    extension<T>(Nullable<T> nullable)
        where T : struct
    {
        public Option<T> ToOption()
        {
            if (nullable.HasValue)
            {
                return nullable.GetValueOrDefault();
            }
            return default;
        }

        public bool TryGetValue([NotNullWhen(true), MaybeNullWhen(false)] out T value)
        {
            value = nullable.GetValueOrDefault();
            return nullable.HasValue;
        }
    }
}