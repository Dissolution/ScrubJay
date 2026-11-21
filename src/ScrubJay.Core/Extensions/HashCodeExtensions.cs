namespace ScrubJay.Extensions;

[PublicAPI]
public static class HashCodeExtensions
{
    private static readonly int _nullHash = HashCode.Combine<object?>(null);
    private static readonly int _emptyHash = new HashCode().ToHashCode();

    extension(HashCode)
    {
        public static int NullHash => _nullHash;

        public static int EmptyHash => _emptyHash;

        [OverloadResolutionPriority(80)]
        public static int Combine<T>(params ReadOnlySpan<T> values)
        {
            int length = values.Length;
            if (length == 0)
                return _emptyHash;
            if (length == 1)
                return HashCode.Combine(values[0]);
            if (length == 2)
                return HashCode.Combine(values[0], values[1]);
            if (length == 3)
                return HashCode.Combine(values[0], values[1], values[2]);
            if (length == 4)
                return HashCode.Combine(values[0], values[1], values[2], values[3]);
            if (length == 5)
                return HashCode.Combine(values[0], values[1], values[2], values[3], values[4]);
            if (length == 6)
                return HashCode.Combine(values[0], values[1], values[2], values[3], values[4], values[5]);
            if (length == 7)
                return HashCode.Combine(values[0], values[1], values[2], values[3], values[4], values[5], values[6]);
            if (length == 8)
                return HashCode.Combine(values[0], values[1], values[2], values[3], values[4], values[5], values[6], values[7]);
            var hasher = new HashCode();
            for (int i = 0; i < length; i++)
            {
                hasher.Add<T>(values[i]);
            }
            return hasher.ToHashCode();
        }

        [OverloadResolutionPriority(100)]
        public static int Combine<T>(T[]? values)
        {
            if (values is null)
                return _nullHash;
            int length = values.Length;
            if (length == 0)
                return _emptyHash;
            if (length == 1)
                return HashCode.Combine(values[0]);
            if (length == 2)
                return HashCode.Combine(values[0], values[1]);
            if (length == 3)
                return HashCode.Combine(values[0], values[1], values[2]);
            if (length == 4)
                return HashCode.Combine(values[0], values[1], values[2], values[3]);
            if (length == 5)
                return HashCode.Combine(values[0], values[1], values[2], values[3], values[4]);
            if (length == 6)
                return HashCode.Combine(values[0], values[1], values[2], values[3], values[4], values[5]);
            if (length == 7)
                return HashCode.Combine(values[0], values[1], values[2], values[3], values[4], values[5], values[6]);
            if (length == 8)
                return HashCode.Combine(values[0], values[1], values[2], values[3], values[4], values[5], values[6], values[7]);
            var hasher = new HashCode();
            for (int i = 0; i < length; i++)
            {
                hasher.Add<T>(values[i]);
            }
            return hasher.ToHashCode();
        }
    }

    extension(ref HashCode hasher)
    {
        [OverloadResolutionPriority(100)]
        public void AddMany<T>(T[]? values)
        {
            if (values is null)
            {
                hasher.Add<object?>(null);
            }
            else
            {
                foreach (var value in values)
                {
                    hasher.Add<T>(value);
                }
            }
        }

        [OverloadResolutionPriority(75)]
        public void AddMany<T>(params ReadOnlySpan<T> values)
        {
            foreach (var value in values)
            {
                hasher.Add<T>(value);
            }
        }
        
        [OverloadResolutionPriority(25)]
        public void AddMany<T>(IEnumerable<T>? values)
        {
            if (values is null)
            {
                hasher.Add<object?>(null);
            }
            else
            {
                foreach (var value in values)
                {
                    hasher.Add<T>(value);
                }
            }
        }
    }
}