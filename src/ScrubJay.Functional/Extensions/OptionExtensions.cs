namespace ScrubJay.Functional.Extensions;

/// <summary>
/// Extensions on <see cref="Option"/>
/// </summary>
[PublicAPI]
public static class OptionExtensions
{
   
    /// <summary>
    /// Extensions on a <c>ref Option&lt;T&gt;</c> that can add/remove/exchange values (similar to rust)
    /// </summary>
    extension<T>(ref Option<T> option)
    {
        /// <summary>
        /// Inserts <paramref name="value"/> into this <see cref="Option{T}"/> and returns what was formerly inside.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Option<T> Insert(T value)
        {
            if (option.IsSome(out var existingValue))
            {
                option = Some(value);
                return Some(existingValue);
            }
            else
            {
                option = Some(value);
                return default;
            }
        }

        public T SomeOrInsert(T value)
        {
            if (option.IsSome(out var existingValue))
                return existingValue;
            option = Some(value);
            return value;
        }

        public T SomeOrInsert(Func<T> valueFactory)
        {
            if (option.IsSome(out var value))
                return value;
            value = valueFactory();
            option = Some(value);
            return value;
        }

        public Option<T> Take()
        {
            if (option.IsSome(out var existingValue))
            {
                option = default;
                return Some(existingValue);
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
                    option = Some(rightValue);
                    right = Some(leftValue);
                }
                else // right is None
                {
                    option = default;
                    right = Some(leftValue);
                }
            }
            else // left is None
            {
                if (right.IsSome(out var rightValue))
                {
                    option = Some(rightValue);
                    right = default;
                }
                else // right is None
                {
                    // no need to do anything
                }
            }
        }
    }
    
    
    public static Option<T> Flatten<T>(this Option<Option<T>> nestedOptions)
    {
        if (nestedOptions.IsSome(out var option))
            return option;

        return default;
    }
}