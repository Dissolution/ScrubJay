using ScrubJay.Functional.Utilities;

namespace ScrubJay.Functional.Extensions;

/// <summary>
/// Extensions on <see cref="Option"/>
/// </summary>
[PublicAPI]
public static class OptionExtensions
{
    extension(Option)
    {
        public static Option<T> Try<T>(Func<T>? func)
        {
            if (func is null)
                return None;

            try
            {
                return Some<T>(func.Invoke());
            }
            catch
            {
                return None;
            }
        }

        public static Option<T> Try<I, T>(
            [NotNullWhen(true)] I? instance,
            [NotNullWhen(true)] Func<I, T>? instanceFunc)
        {
            if (instance is null || instanceFunc is null)
                return None;

            try
            {
                return Some<T>(instanceFunc.Invoke(instance));
            }
            catch
            {
                return None;
            }
        }

        public static Option<T> Try<T>(TryInvoke<T>? tryInvoke)
        {
            if (tryInvoke is null)
                return None;

            bool ok;
            T? value;

            try
            {
                ok = tryInvoke(out value);
            }
            catch
            {
                return None;
            }

            if (ok)
                return Some<T>(value);
            return None;
        }
    }
}