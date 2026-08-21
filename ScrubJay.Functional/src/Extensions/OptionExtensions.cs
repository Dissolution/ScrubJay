namespace ScrubJay.Functional.Extensions;

/// <summary>
/// Extensions on <see cref="Option{T}"/>
/// </summary>
[PublicAPI]
public static class OptionExtensions
{
    public static Option<T> Flatten<T>(this Option<Option<T>> nestedOptions)
    {
        if (nestedOptions.IsSome(out var option))
            return option;
        return default;
    }

    /// <summary>
    /// Extensions on a <c>ref Option&lt;T&gt;</c> that can add/remove/exchange values (similar to rust)
    /// </summary>
    /// <param name="option"></param>
    /// <typeparam name="T"></typeparam>
    extension<T>(ref Option<T> option)
    {
        public Option<T> Insert(T value)
        {
            if (option.IsSome(out var existingValue))
            {
                option = Option.Some(value);
                return Option.Some(existingValue);
            }
            else
            {
                option = Option.Some(value);
                return default;
            }
        }

        public T GetOrInsert(T value)
        {
            if (option.IsSome(out var existingValue))
                return existingValue;
            option = Option.Some(value);
            return value;
        }

        public T GetOrInsert(Func<T> valueFactory)
        {
            if (option.IsSome(out var value))
                return value;
            value = valueFactory();
            option = Option.Some(value);
            return value;
        }

        public Option<T> Take()
        {
            if (option.IsSome(out var existingValue))
            {
                option = default;
                return Option.Some(existingValue);
            }
            else
            {
                option = default;
                return default;
            }
        }

        public void Swap(ref Option<T> right)
        {
            if (option.IsSome(out var leftValue))
            {
                if (right.IsSome(out var rightValue))
                {
                    option = Option.Some(rightValue);
                    right = Option.Some(leftValue);
                }
                else // right is None
                {
                    option = default;
                    right = Option.Some(leftValue);
                }
            }
            else // left is None
            {
                if (right.IsSome(out var rightValue))
                {
                    option = Option.Some(rightValue);
                    right = default;
                }
                else // right is None
                {
                    // no need to do anything
                }
            }
        }
    }
}