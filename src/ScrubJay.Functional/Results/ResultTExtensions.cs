namespace ScrubJay.Functional;

[PublicAPI]
public static class ResultTExtensions
{
    extension<T>(Result<T> result)
    {
        [StackTraceHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ResultAwaiter<T> GetAwaiter() => new(result);
    }
}