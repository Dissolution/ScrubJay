// ReSharper disable InvokeAsExtensionMember

namespace ScrubJay.Text.Comparison;

[PublicAPI]
public sealed class TextComparison :
    IEqualityComparer<char>, IComparer<char>,
    IEqualityComparer<string>, IComparer<string>,
#if NET9_0_OR_GREATER
    IEqualityComparer<text>, IComparer<text>,
#endif
    IEqualityComparer, IComparer
{
    public static implicit operator StringComparison(TextComparison tc) => tc._stringComparison;

    public static implicit operator StringComparer(TextComparison tc) => tc._stringComparison switch
    {
        StringComparison.CurrentCulture => StringComparer.CurrentCulture,
        StringComparison.CurrentCultureIgnoreCase => StringComparer.CurrentCultureIgnoreCase,
        StringComparison.InvariantCulture => StringComparer.InvariantCulture,
        StringComparison.InvariantCultureIgnoreCase => StringComparer.InvariantCultureIgnoreCase,
        StringComparison.Ordinal => StringComparer.Ordinal,
        StringComparison.OrdinalIgnoreCase => StringComparer.OrdinalIgnoreCase,
        _ => Ordinal,
    };

    public static implicit operator TextComparison(StringComparison comparison) => comparison switch
    {
        StringComparison.CurrentCulture => Current,
        StringComparison.CurrentCultureIgnoreCase => CurrentIgnoreCase,
        StringComparison.InvariantCulture => Invariant,
        StringComparison.InvariantCultureIgnoreCase => InvariantIgnoreCase,
        StringComparison.Ordinal => Ordinal,
        StringComparison.OrdinalIgnoreCase => OrdinalIgnoreCase,
        _ => Ordinal,
    };

    public static readonly TextComparison Ordinal = new(StringComparison.Ordinal);
    public static readonly TextComparison OrdinalIgnoreCase = new(StringComparison.OrdinalIgnoreCase);
    public static readonly TextComparison Invariant = new(StringComparison.InvariantCulture);
    public static readonly TextComparison InvariantIgnoreCase = new(StringComparison.InvariantCultureIgnoreCase);
    public static readonly TextComparison Current = new(StringComparison.CurrentCulture);
    public static readonly TextComparison CurrentIgnoreCase = new(StringComparison.CurrentCultureIgnoreCase);

    private readonly StringComparison _stringComparison;

    private TextComparison(StringComparison stringComparison)
    {
        _stringComparison = stringComparison;
    }

#region Equals
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(char x, char y)
    {
        return MemoryExtensions.Equals(x.AsSpan(), y.AsSpan(), _stringComparison);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(char x, string? y)
    {
        return MemoryExtensions.Equals(x.AsSpan(), y.AsSpan(), _stringComparison);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(char x, text y)
    {
        return MemoryExtensions.Equals(x.AsSpan(), y, _stringComparison);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(string? x, char y)
    {
        return MemoryExtensions.Equals(x.AsSpan(), y.AsSpan(), _stringComparison);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(string? x, string? y)
    {
        return string.Equals(x, y, _stringComparison);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(string? x, text y)
    {
        return MemoryExtensions.Equals(x.AsSpan(), y, _stringComparison);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(text x, char y)
    {
        return MemoryExtensions.Equals(x, y.AsSpan(), _stringComparison);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(text x, string? y)
    {
        return MemoryExtensions.Equals(x, y.AsSpan(), _stringComparison);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(text x, text y)
    {
        return MemoryExtensions.Equals(x, y, _stringComparison);
    }

    bool IEqualityComparer.Equals(object? x, object? y)
    {
        if (!TextHelper.TryUnboxText(x, out var xText))
            xText = x?.ToString();
        if (!TextHelper.TryUnboxText(y, out var yText))
            yText = y?.ToString();
        return Equals(xText, yText);
    }
#endregion

#region GetHashCode
#if NET6_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetHashCode(char ch)
    {
        return string.GetHashCode(ch.AsSpan(), _stringComparison);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetHashCode(string? str)
    {
        return string.GetHashCode(str.AsSpan(), _stringComparison);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetHashCode(text text)
    {
        return string.GetHashCode(text, _stringComparison);
    }
#else
    public int GetHashCode(char ch)
    {
        return ((StringComparer)this).GetHashCode(new string(ch, 1));
    }

    public int GetHashCode(string? str)
    {
        if (str is null) return 0;
        return ((StringComparer)this).GetHashCode(str);
    }
    
    public int GetHashCode(text text)
    {
        return ((StringComparer)this).GetHashCode(text.ToString());
    }
#endif

    int IEqualityComparer.GetHashCode(object? obj)
    {
        if (!TextHelper.TryUnboxText(obj, out var text))
            text = obj?.ToString();
        return GetHashCode(text);
    }
#endregion

#region Compare
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Compare(char x, char y)
    {
        return MemoryExtensions.CompareTo(x.AsSpan(), y.AsSpan(), _stringComparison);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Compare(char x, string? y)
    {
        return MemoryExtensions.CompareTo(x.AsSpan(), y.AsSpan(), _stringComparison);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Compare(char x, text y)
    {
        return MemoryExtensions.CompareTo(x.AsSpan(), y, _stringComparison);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Compare(string? x, char y)
    {
        return MemoryExtensions.CompareTo(x.AsSpan(), y.AsSpan(), _stringComparison);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Compare(string? x, string? y)
    {
        return MemoryExtensions.CompareTo(x.AsSpan(), y.AsSpan(), _stringComparison);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Compare(string? x, text y)
    {
        return MemoryExtensions.CompareTo(x.AsSpan(), y, _stringComparison);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Compare(text x, char y)
    {
        return MemoryExtensions.CompareTo(x, y.AsSpan(), _stringComparison);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Compare(text x, string? y)
    {
        return MemoryExtensions.CompareTo(x, y.AsSpan(), _stringComparison);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Compare(text x, text y)
    {
        return MemoryExtensions.CompareTo(x, y, _stringComparison);
    }

    int IComparer.Compare(object? x, object? y)
    {
        if (!TextHelper.TryUnboxText(x, out var xText))
            xText = x?.ToString();
        if (!TextHelper.TryUnboxText(y, out var yText))
            yText = y?.ToString();
        return Compare(xText, yText);
    }
#endregion
}