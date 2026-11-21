namespace ScrubJay.Collections.Pooling;

[PublicAPI]
public class BoundedObjectPool<T> : ObjectPool<T>
    where T : class
{
    private readonly int _maxRetained;
    private int _numRetained;
    private readonly Action<T>? _disposeInstance;

    public BoundedObjectPool(int maxRetained,
        Func<T> createInstance,
        Action<T>? cleanInstance,
        Action<T>? disposeInstance)
        : base(createInstance, cleanInstance)
    {
        _maxRetained = maxRetained;
        _disposeInstance = disposeInstance;
    }

    protected override T RentSlow()
    {
        if (_overflow.TryDequeue(out var instance))
        {
            Interlocked.Decrement(ref _numRetained);
            return instance;
        }

        return _createInstance();
    }

    protected override void ReturnSlow(T instance)
    {
        if (Interlocked.Increment(ref _numRetained) <= _maxRetained)
        {
            _overflow.Enqueue(instance);
        }
        else
        {
            // rollback
            Interlocked.Decrement(ref _numRetained);
            _disposeInstance?.Invoke(instance);
        }
    }
}