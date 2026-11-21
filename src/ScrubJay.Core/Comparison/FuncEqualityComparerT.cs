namespace ScrubJay.Comparison;

[PublicAPI]
public sealed class FuncEqualityComparer<T> : IEqualityComparer<T>, IEqualityComparer
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
{
    private readonly Func<T?, T?, bool> _equals;
    private readonly Func<T, int> _getHashCode;

    public FuncEqualityComparer(Func<T?, T?, bool> equals, Func<T, int> getHashCode)
    {
        _equals = equals;
        _getHashCode = getHashCode;
    }

    bool IEqualityComparer.Equals(object? objLeft, object? objRight)
    {
        if (objLeft is T left && objRight is T right)
            return _equals(left, right);
        return false;
    }

    int IEqualityComparer.GetHashCode(object obj)
    {
        if (obj is T value)
            return _getHashCode(value);
        return HashCode.NullHash;
    }

    public bool Equals(T? left, T? right) => _equals(left, right);

    public int GetHashCode([DisallowNull] T value) => _getHashCode(value);
}

[PublicAPI]
public sealed class FuncComparer<T> : IComparer<T>, IComparer
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
{
    private readonly Func<T?, T?, int> _compare;

    public FuncComparer(Func<T?, T?, int> compare)
    {
        _compare = compare;
    }

    int IComparer.Compare(object? objLeft, object? objRight)
    {
        if (objLeft is T left)
        {
            if (objRight is T right)
            {
                return _compare(left, right);
            }
            else
            {
                // T sorts after non-T
                return 1;
            }
        }
        else
        {
            if (objRight is T)
            {
                // non-T sorts before T
                return -1;
            }
            else
            {
                return 0; // as far as we care, they are equal
            }
        }
    }

    public int Compare(T? x, T? y) => _compare(x, y);
}