using Microsoft.Extensions.ObjectPool;

namespace ScrubJay.Text.Pooling;

[PublicAPI]
public static class ObjectPoolExtensions
{
    extension<T>(ObjectPool<T> pool)
        where T : class
    {
        public void Borrow(Action<T> rentedInstanceAction)
        {
            T instance = pool.Get();
            rentedInstanceAction(instance);
            pool.Return(instance);
        }

        public R Borrow<R>(Func<T, R> rentedInstanceFunc)
#if NET9_0_OR_GREATER
            where R : allows ref struct
#endif
        {
            T instance = pool.Get();
            R result = rentedInstanceFunc(instance);
            pool.Return(instance);
            return result;
        }
    }

}