using Microsoft.Extensions.ObjectPool;

namespace ScrubJay.Universal;

[PublicAPI]
public static class StringBuilderPool
{
    private static readonly ObjectPool<StringBuilder> _pool =
        ObjectPool.Create<StringBuilder>(new StringBuilderPooledObjectPolicy());

    extension(StringBuilder)
    {
        /// <summary>
        /// Rents a <see cref="StringBuilder"/> instance from a shared pool.
        /// </summary>
        public static StringBuilder Rent() => _pool.Get();
    }

    extension(StringBuilder builder)
    {
        /// <summary>
        /// Returns this <see cref="StringBuilder"/> instance to a shared pool.
        /// </summary>
        public void Return() => _pool.Return(builder);

        /// <summary>
        /// Returns this <see cref="StringBuilder"/> instance to a shared pool
        /// and then returns the <see cref="string"/> it created.
        /// </summary>
        public string ToStringAndReturn()
        {
            string str = builder.ToString();
            _pool.Return(builder);
            return str;
        }
    }
}