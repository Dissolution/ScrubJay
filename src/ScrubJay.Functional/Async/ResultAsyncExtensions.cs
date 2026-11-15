namespace ScrubJay.Functional.Async;

[PublicAPI]
public static class ResultAsyncExtensions
{
    public static ResultAwaiter<T> GetAwaiter<T>(this Result<T, Exception> result) => new(result);
}