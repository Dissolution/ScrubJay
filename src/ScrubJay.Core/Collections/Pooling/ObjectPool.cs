#pragma warning disable CA1051, CA1716

using System.Collections.Concurrent;

namespace ScrubJay.Collections.Pooling;

[PublicAPI]
public class ObjectPool<T>
    where T : class
{
    protected readonly ConcurrentQueue<T> _overflow = new();
    protected T? _fastItem;

    protected readonly Func<T> _createInstance;
    protected readonly Action<T>? _cleanInstance;

    protected ObjectPool(Func<T> createInstance, Action<T>? cleanInstance)
    {
        _createInstance = createInstance;
        _cleanInstance = cleanInstance;
    }

    public virtual T Rent()
    {
        T? item = _fastItem;
        if (item is not null && Interlocked.CompareExchange<T?>(ref _fastItem, null, item) == item)
        {
            return item;
        }

        return RentSlow();
    }

    protected virtual T RentSlow()
    {
        if (_overflow.TryDequeue(out var item))
        {
            return item;
        }

        return _createInstance();
    }

    public virtual void Return(T? instance)
    {
        if (instance is null)
            return;

        _cleanInstance?.Invoke(instance);
        if (_fastItem is null && Interlocked.CompareExchange(ref _fastItem, instance, null) is null)
        {
            return;
        }

        ReturnSlow(instance);
    }

    protected virtual void ReturnSlow(T instance)
    {
        _overflow.Enqueue(instance);
    }
}