namespace ScrubJay.Functional;

[PublicAPI]
public struct ResultAsyncMethodBuilder<T>
{
    public static ResultAsyncMethodBuilder<T> Create() => new();

    private Action CreateCompletionAction<TStateMachine>(
        ref TStateMachine stateMachine)
        where TStateMachine : IAsyncStateMachine
    {
        var boxedStateMachine = stateMachine;
        return boxedStateMachine.MoveNext;
    }
    
    private bool? _isOk;
    private T? _value;
    private Exception? _error;
    
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

        // should call when completes
        //stateMachine.MoveNext();

        var completionActino = CreateCompletionAction<TStateMachine>(ref stateMachine);
        awaiter.OnCompleted(completionActino);
    }

    public void AwaitUnsafeOnCompleted<A, SM>(
        ref A awaiter, 
        ref SM stateMachine)
        where A : ICriticalNotifyCompletion
        where SM : IAsyncStateMachine
    {
        // if the state machine reaches an `await expr` expression, 
        // GetAwaiter() is called, then this method is called if IsCompleted == false
        
        // should call when completes
        stateMachine.MoveNext();

        //IAsyncStateMachine iasm = stateMachine;
        //awaiter.UnsafeOnCompleted(iasm.MoveNext);
        
//        var completionActino = CreateCompletionAction<TStateMachine>(ref stateMachine);
//        awaiter.UnsafeOnCompleted(completionActino);
    }
}