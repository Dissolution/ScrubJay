namespace ScrubJay.Functional.Extensions;

/// <summary>
/// Extensions on <see cref="Nullable{T}"/> instances.
/// </summary>
[PublicAPI]
public static class NullableExtensions
{
    extension<S>(Nullable<S> nullable)
        where S : struct
    {
        public bool TryGetValue(out S value)
        {
            if (nullable.HasValue)
            {
                value = nullable.GetValueOrDefault();
                return true;
            }
            value = default;
            return false;
        }

        public Option<S> ToOption()
        {
            if (nullable.HasValue)
            {
                return Option.Some(nullable.GetValueOrDefault());
            }
            return default;
        }
    }
}