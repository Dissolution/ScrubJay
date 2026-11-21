namespace ScrubJay.Text.Comparison;

[PublicAPI]
public abstract class TextComparer :
    ITextEqualityComparer, IEqualityComparer,
    ITextComparer, IComparer
{
#region Static Instances

    public static OrdinalTextComparer Ordinal { get; } = new OrdinalTextComparer();

    public static OrdinalIgnoreCaseTextComparer OrdinalIgnoreCase { get; } = new OrdinalIgnoreCaseTextComparer();

    public static CulturedTextComparer Invariant { get; } = new CulturedTextComparer(StringComparison.InvariantCulture);

    public static CulturedTextComparer InvariantIgnoreCase { get; } = new CulturedTextComparer(StringComparison.InvariantCultureIgnoreCase);

    public static CulturedTextComparer Current { get; } = new CulturedTextComparer(StringComparison.CurrentCulture);

    public static CulturedTextComparer CurrentIgnoreCase { get; } = new CulturedTextComparer(StringComparison.CurrentCultureIgnoreCase);

    public static StringComparisonTextComparer FromComparison(StringComparison comparison) => comparison switch
    {
        StringComparison.CurrentCulture => Current,
        StringComparison.CurrentCultureIgnoreCase => CurrentIgnoreCase,
        StringComparison.InvariantCulture => Invariant,
        StringComparison.InvariantCultureIgnoreCase => InvariantIgnoreCase,
        StringComparison.Ordinal => Ordinal,
        StringComparison.OrdinalIgnoreCase => OrdinalIgnoreCase,
        _ => throw new ArgumentOutOfRangeException(nameof(comparison), comparison, null),
    };

#endregion

#region Equals

    bool IEqualityComparer.Equals(object? x, object? y)
    {
        if (TextHelper.TryUnboxText(x, out text xText))
        {
            if (TextHelper.TryUnboxText(y, out text yText))
            {
                return Equals(xText, yText);
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (TextHelper.CanUnboxText(y))
            {
                return false;
            }
            else
            {
                // as far as we care, they are equal
                return true;
            }
        }
    }

    public virtual bool Equals(char x, char y) => Equals(x.AsSpan(), y.AsSpan());

    public virtual bool Equals(string? x, string? y)
    {
        if (x is null)
            return y is null;
        if (y is null)
            return false;
        return Equals(x.AsSpan(), y.AsSpan());
    }

    public virtual bool Equals(char[]? x, char[]? y)
    {
        if (x is null)
            return y is null;
        if (y is null)
            return false;
        return Equals(x.AsSpan(), y.AsSpan());
    }

    public abstract bool Equals(text x, text y);

#endregion

#region GetHashCode

    int IEqualityComparer.GetHashCode(object? obj)
    {
        if (TextHelper.TryUnboxText(obj, out text txt))
            return GetHashCode(txt);
        return HashCode.NullHash;
    }

    public virtual int GetHashCode(char ch) => GetHashCode(ch.AsSpan());

    public virtual int GetHashCode(string? str)
    {
        if (str is null)
            return HashCode.NullHash;
        return GetHashCode(str.AsSpan());
    }

    public virtual int GetHashCode(char[]? chars)
    {
        if (chars is null)
            return HashCode.NullHash;
        return GetHashCode(chars.AsSpan());
    }

    public abstract int GetHashCode(text obj);

#endregion

#region Compare

    int IComparer.Compare(object? x, object? y)
    {
        if (TextHelper.TryUnboxText(x, out var xText))
        {
            if (TextHelper.TryUnboxText(y, out var yText))
            {
                return Compare(xText, yText);
            }
            else
            {
                // text sorts after non-text
                return 1;
            }
        }
        else
        {
            if (TextHelper.CanUnboxText(x))
            {
                // non-text sorts after text
                return -1;
            }
            else
            {
                // as far as we care, they are equal
                return 0;
            }
        }
    }

    public virtual int Compare(char x, char y)
        => Compare(x.AsSpan(), y.AsSpan());

    public virtual int Compare(string? x, string? y)
    {
        if (x is not null)
        {
            if (y is not null)
            {
                return Compare(x.AsSpan(), y.AsSpan());
            }
            else
            {
                return 1;
            }
        }
        else
        {
            if (y is not null)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }

    public virtual int Compare(char[]? x, char[]? y)
    {
        if (x is not null)
        {
            if (y is not null)
            {
                return Compare(x.AsSpan(), y.AsSpan());
            }
            else
            {
                return 1;
            }
        }
        else
        {
            if (y is not null)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }

    public abstract int Compare(text x, text y);

#endregion
}