using System.Globalization;

namespace ScrubJay.Functional;

/// <summary>
/// Extensions on <see cref="Result"/>
/// </summary>
[PublicAPI]
public static class ResultExtensions
{
    public delegate bool TryInvoke<T>(out T value);


    extension(Result)
    {
#if NET7_0_OR_GREATER
        public static Result<T> Parse<T>(
            scoped ReadOnlySpan<char> text,
            IFormatProvider? provider = null)
            where T : ISpanParsable<T>
        {
            if (T.TryParse(text, provider, out var value))
                return value;
            return new ArgumentException($"Could not parse '{text}' to a {typeof(T)} value", nameof(text));
        }

        public static Result<T> Parse<T>(
            string? str,
            IFormatProvider? provider = null)
            where T : IParsable<T>
        {
            if (T.TryParse(str, provider, out var value))
                return value;
            return new ArgumentException($"Could not parse \"{str}\" to a {typeof(T)} value", nameof(str));
        }

        public static Result<N> Parse<N>(
            scoped ReadOnlySpan<char> text,
            NumberStyles numberStyle = NumberStyles.Number,
            IFormatProvider? provider = null)
            where N : INumberBase<N>
        {
            if (N.TryParse(text, numberStyle, provider, out var value))
                return value;
            return new ArgumentException($"Could not parse '{text}' to a {typeof(N)} number", nameof(text));
        }

        public static Result<N> Parse<N>(
            string? str,
            NumberStyles numberStyle = NumberStyles.Number,
            IFormatProvider? provider = null)
            where N : INumberBase<N>
        {
            if (N.TryParse(str, numberStyle, provider, out var value))
                return value;
            return new ArgumentException($"Could not parse \"{str}\" to a {typeof(N)} number", nameof(str));
        }
#endif

        /// <summary>
        /// Try to invoke an <see cref="Action"/>
        /// </summary>
        /// <param name="action"></param>
        /// <returns>
        /// The <see cref="Result{T}"/> of the invocation
        /// </returns>
        public static Result Try(Action? action)
        {
            if (action is null)
            {
                return new ArgumentNullException(nameof(action));
            }

            try
            {
                action.Invoke();
                return true;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public static Result Try<I>(
            [NotNullWhen(true)] I? instance,
            [NotNullWhen(true)] Action<I>? instanceAction)
        {
            if (instance is null)
                return new ArgumentNullException(nameof(instance));

            if (instanceAction is null)
                return new ArgumentNullException(nameof(instanceAction));

            try
            {
                instanceAction.Invoke(instance);
                return true;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }


        public static Result<T> Try<T>(Func<T>? func)
        {
            if (func is null)
                return new ArgumentNullException(nameof(func));

            try
            {
                var value = func.Invoke();
                return Result<T>.Ok(value);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public static Result<T> Try<I, T>(
            [NotNullWhen(true)] I? instance,
            [NotNullWhen(true)] Func<I, T>? instanceFunc)
        {
            if (instance is null)
                return new ArgumentNullException(nameof(instance));

            if (instanceFunc is null)
                return new ArgumentNullException(nameof(instanceFunc));

            try
            {
                var value = instanceFunc.Invoke(instance);
                return Result<T>.Ok(value);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public static Result<T> Try<T>(TryInvoke<T>? tryInvoke)
        {
            if (tryInvoke is null)
                return new ArgumentNullException(nameof(tryInvoke));

            try
            {
                bool b = tryInvoke(out var value);
                if (b)
                    return Result<T>.Ok(value);
                return Result<T>.Error(new InvalidOperationException());
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }

    extension<T1, T2>(in Result<(T1, T2)> result)
    {
        public bool IsOk([MaybeNullWhen(false)] out T1 item1, [MaybeNullWhen(false)] out T2 item2)
        {
            if (result.IsOk(out var tuple))
            {
                item1 = tuple.Item1;
                item2 = tuple.Item2;
                return true;
            }
            else
            {
                item1 = default;
                item2 = default;
                return false;
            }
        }

        public bool IsOk(
            [MaybeNullWhen(false)] out T1 item1,
            [MaybeNullWhen(false)] out T2 item2,
            [NotNullWhen(false)] out Exception? error)
        {
            if (result.IsOk(out var tuple, out error))
            {
                item1 = tuple.Item1;
                item2 = tuple.Item2;
                return true;
            }
            else
            {
                item1 = default;
                item2 = default;
                return false;
            }
        }
    }

    extension<T1, T2, T3>(in Result<(T1, T2, T3)> result)
    {
        public bool IsOk(
            [MaybeNullWhen(false)] out T1 item1,
            [MaybeNullWhen(false)] out T2 item2,
            [MaybeNullWhen(false)] out T3 item3)
        {
            if (result.IsOk(out var tuple))
            {
                item1 = tuple.Item1;
                item2 = tuple.Item2;
                item3 = tuple.Item3;
                return true;
            }
            else
            {
                item1 = default;
                item2 = default;
                item3 = default;
                return false;
            }
        }
    }
}