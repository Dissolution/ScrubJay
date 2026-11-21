namespace ScrubJay.Collections.Pooling;

[PublicAPI]
public static class StringBuilderExtensions
{
    private static readonly BoundedObjectPool<StringBuilder> _pool = new(Environment.ProcessorCount * 2, static () => new StringBuilder(), static sb => sb.Clear(), null);

    extension(StringBuilder)
    {
        public static StringBuilder Rent() => _pool.Rent();

        public static StringBuilder Rent(int minCapacity)
        {
            var sb = _pool.Rent();
            if (sb.Capacity < minCapacity)
            {
                _ = sb.EnsureCapacity(minCapacity);
            }

            return sb;
        }
    }

    extension(StringBuilder? stringBuilder)
    {
        public void Dispose()
        {
            _pool.Return(stringBuilder);
        }

        public void Return()
        {
            _pool.Return(stringBuilder);
        }

        public string ToStringAndReturn()
        {
            if (stringBuilder is not null)
            {
                string str = stringBuilder.ToString();
                _pool.Return(stringBuilder);
                return str;
            }

            return string.Empty;
        }
    }
}