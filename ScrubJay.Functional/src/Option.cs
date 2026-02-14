#pragma warning disable CA1716

namespace ScrubJay.Functional;

/// <summary>
/// A static utility class for constructing <see cref="Option{T}"/> instances
/// </summary>
[PublicAPI]
public static class Option
{
#region Constructors

    /// <summary>
    /// Gets a <see cref="IMPL.None"/> that implicitly converts into any <see cref="Option{T}.None"/>
    /// </summary>
    /// <returns></returns>
    public static IMPL.None None() => default;

    /// <inheritdoc cref="Option{T}.None"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> None<T>() => Option<T>.None;

    /// <inheritdoc cref="Option{T}.Some"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> Some<T>(T value) => Option<T>.Some(value);

    /// <inheritdoc cref="Option{T}.SomeIf"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> SomeIf<T>(T value, Func<T, bool> predicate)
        => Option<T>.SomeIf(value, predicate);

#if NET9_0_OR_GREATER
    /// <inheritdoc cref="Option{T}.SomeIf"/>
    public static RefOption<T> SomeIf<T>(T value, Func<T, bool> predicate,
        // ReSharper disable once MethodOverloadWithOptionalParameter
        TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
        => RefOption<T>.SomeIf(value, predicate);
#endif

    /// <summary>
    /// Returns <see cref="Option{T}.Some"/> if <paramref name="value"/> is not <c>null</c>,<br/>
    /// otherwise returns <see cref="Option{T}.None"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> NotNull<T>(T? value)
    {
        if (value is not null)
            return Option<T>.Some(value);
        return Option<T>.None;
    }

    /// <summary>
    /// Returns <see cref="Option{T}.Some"/> if <paramref name="value"/> is not <c>null</c>,<br/>
    /// otherwise returns <see cref="Option{T}.None"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // ReSharper disable once ConvertNullableToShortForm
    public static Option<T> NotNull<T>(Nullable<T> value)
        where T : struct
    {
        if (value.HasValue)
        {
            return Option<T>.Some(value.GetValueOrDefault());
        }

        return Option<T>.None;
    }

#endregion

#region Extensions

    public static Option<T> Flatten<T>(this Option<Option<T>> nestedOptions)
    {
        if (nestedOptions.IsSome(out var option))
            return option;
        return default;
    }

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

#endregion
}