namespace ScrubJay.Functional;

[PublicAPI]
[StackTraceHidden]
public struct ResultAwaiter<T> : INotifyCompletion, ICriticalNotifyCompletion
{
    private bool? _isOk;
    private T? _value;
    private Exception? _error;

    public bool IsCompleted
    {
        get
        {
            bool isCompeted = _isOk is not null;
            if (!isCompeted)
            {
                Debugger.Break();
            }
            return isCompeted;
        }
    }

    public ResultAwaiter(Result<T> result)
    {
        _isOk = result._isOk;
        _value = result._value;
        _error = result._error;
        if (_isOk == true)
        {
            if (_value is null)
            {
                Debugger.Break();
            }
        }
        else if (_isOk == false)
        {
            if (_error is null)
            {
                Debugger.Break();
            }
        }
    }

    [StackTraceHidden]
    public void OnCompleted(Action continuation)
    {
        if (_isOk == false)
        {
            throw _error!;
        }
        else if (_isOk == true)
        {
            continuation.Invoke();
        }
        else
        {
            Debugger.Break();
        }
    }

    [StackTraceHidden]
    public void UnsafeOnCompleted(Action continuation)
    {
        if (_isOk == false)
        {
            throw _error!;
        }
        else if (_isOk == true)
        {
            continuation.Invoke();
        }
        else
        {
            Debugger.Break();
        }
    }

    [StackTraceHidden]
    public T GetResult()
    {
        if (_isOk == true)
        {
            return _value!;
        }
        else if (_isOk == false)
        {
            throw _error!;
        }
        else
        {
            Debugger.Break();
            throw new NotImplementedException();
        }
    }
}