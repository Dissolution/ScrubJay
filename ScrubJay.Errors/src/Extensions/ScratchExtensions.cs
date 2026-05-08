namespace ScrubJay.Errors.Extensions;

public static class ScratchExtensions
{
    extension(Activator)
    {
        public static Result<T> TryCreateInstance<T>(params object?[]? args)
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

        public static Result<T> TryCreateInstance<T>(Type instanceType, params object?[]? args)
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
        
        public static Result<object?> TryCreateInstance(Type? instanceType, params object?[]? args)
        {
            try
            {
                return Activator.CreateInstance(instanceType, args)!;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}