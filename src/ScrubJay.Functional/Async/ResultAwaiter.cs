namespace ScrubJay.Functional.Async;

[PublicAPI]
[StructLayout(LayoutKind.Auto)]
public readonly record struct ResultAwaiter<T> : ICriticalNotifyCompletion, INotifyCompletion
{
    public readonly Result<T, Exception> Result;

    public bool IsCompleted => true;

    public ResultAwaiter(Result<T, Exception> result)
    {
        Result = result;
    }

    public void OnCompleted(Action? continuation) => continuation?.Invoke();

    public void UnsafeOnCompleted(Action? continuation) => continuation?.Invoke();

    public T GetResult() => Result.OkOrThrow();
}