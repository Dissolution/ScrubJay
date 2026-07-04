namespace ScrubJay.Functional.Extensions;

public static class PolyfillExtensions
{
    extension(Activator)
    {
        public static Result<T, Exception> TryCreateInstance<T>(params object?[]? args)
        {
            try
            {
                return (T)Activator.CreateInstance(typeof(T), args)!;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public static Result<T, Exception> TryCreateInstance<T>(Type instanceType, params object?[]? args)
        {
            try
            {
                return (T)Activator.CreateInstance(instanceType, args)!;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public static Result<object?, Exception> TryCreateInstance(Type? instanceType, params object?[]? args)
        {
            try
            {
                return Activator.CreateInstance(instanceType!, args)!;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }

    extension<N>(Nullable<N> nullable)
        where N : struct
    {
        public Option<N> ToOption()
        {
            if (nullable.HasValue)
            {
                return nullable.GetValueOrDefault(); // fastest path
            }
            return default;
        }
    }
}