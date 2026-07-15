using ScrubJay.Debugging;

namespace ScrubJay.Functional;

[PublicAPI]
public static class ResultTExtensions
{
    extension<T>(Result<T> result)
    {
        public ResultAwaiter<T> GetAwaiter() => new ResultAwaiter<T>(result);
    }
}

public class ResultAwaiter<T> : INotifyCompletion, ICriticalNotifyCompletion
{
    protected readonly Result<T> _result;

    public bool IsCompleted
    {
        get
        {
            Debugger.Break();
            return true;
        }
    }
        
    public ResultAwaiter(Result<T> result)
    {
        _result = result;
        // result is not actually constructed yet?
        if (result._isOk || result._error is not null)
            Debugger.Break();
    }

    public void OnCompleted(Action continuation)
    {
        // do nothing
        Debugger.Break();
    }

    public void UnsafeOnCompleted(Action continuation)
    {
        // do nothing
        Debugger.Break();
    }

    public T GetResult()
    {
        Debugger.Break();
        return _result.OkOrThrow();
    }
}

public class ResultAsyncMethodBuilder<T>
{
    public static ResultAsyncMethodBuilder<T> Create() => new();

    private bool? _isOk = null;
    private T? _value = default;
    private Exception? _error = default;
    
    // after Start() returns, the async method calls this for the awaitable to return from the async method
    public Result<T> Task
    {
        get
        {
            if (_isOk == true)
            {
                return Result<T>.Ok(_value!);
            }
            else if (_isOk == false)
            {
                if (_error is not null)
                {
                    return Result<T>.Error(_error);
                }
                else
                {
                    Debugger.Break();
                    throw new NotImplementedException();
                }
            }
            else
            {
                Debugger.Break();
                throw new NotImplementedException();
            }
        }
    }
    
    public void Start<TStateMachine>(ref TStateMachine stateMachine)
        where TStateMachine : IAsyncStateMachine
    {
        // associates this builder with the compiler-generated state machine instance

        // must be called to advance the state machine
        stateMachine.MoveNext();
    }

    public void SetStateMachine(IAsyncStateMachine stateMachine)
    {
        // if the statemachine is a struct, it will box itself and call this method
        // we can cache this if necessary
        Debugger.Break();
    }

    public void SetException(Exception exception)
    {
        // any exception is thrown in the state machine
        _error = exception;
        _isOk = false;
    }

    public void SetResult(T result)
    {
        // completed successfully
        _value = result;
        _isOk = true;
    }

    public void AwaitOnCompleted<TAwaiter, TStateMachine>(
        ref TAwaiter awaiter, 
        ref TStateMachine stateMachine)
        where TAwaiter : INotifyCompletion
        where TStateMachine : IAsyncStateMachine
    {
        // if the state machine reaches an `await expr` expression, 
        // GetAwaiter() is called, then this method is called
        Debugger.Break();
        // should call when completes
        stateMachine.MoveNext();
        Debugger.Break();
    }

    public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(
        ref TAwaiter awaiter, 
        ref TStateMachine stateMachine)
        where TAwaiter : ICriticalNotifyCompletion
        where TStateMachine : IAsyncStateMachine
    {
        // if the state machine reaches an `await expr` expression, 
        // GetAwaiter() is called, then this method is called if IsCompleted == false
        Debugger.Break();
        // should call when completes
        stateMachine.MoveNext();
        Debugger.Break();
    }
}