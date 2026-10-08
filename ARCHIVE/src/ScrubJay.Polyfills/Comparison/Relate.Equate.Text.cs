// ReSharper disable InvokeAsExtensionMember

namespace ScrubJay.Polyfills.Comparison;

public static partial class Relate
{
#region Equate(char, char, ?)
    public static bool Equate(char left, char right)
    {
        return left == right;
    }

    public static bool Equate(char left, char right, StringComparison comparison)
    {
        if (comparison == StringComparison.Ordinal)
        {
            return left == right;
        }
        else
        {
            return Equate(left.AsSpan(), right.AsSpan(), comparison);
        }
    }

    public static bool Equate(char left, char right, IEqualityComparer<char>? charComparer)
    {
        if (charComparer is not null)
        {
            return charComparer.Equals(left, right);
        }
        else
        {
            return left == right;
        }
    }

    public static bool Equate(char left, char right, IEqualityComparer<string>? stringComparer)
    {
        if (stringComparer is not null)
        {
            return stringComparer.Equals(left.ToString(), right.ToString());
        }
        else
        {
            return left == right;
        }
    }

#if NET9_0_OR_GREATER
    public static bool Equate(char left, char right, IEqualityComparer<text>? stringComparer)
    {
        if (stringComparer is not null)
        {
            return stringComparer.Equals(left.AsSpan(), right.AsSpan());
        }
        else
        {
            return left == right;
        }
    }
#endif
#endregion
    
#region Equate(text, text, ?)
    public static bool Equate(scoped text left, scoped text right)
    {
        return MemoryExtensions.Equals(left, right, StringComparison.Ordinal);
    }

    public static bool Equate(scoped text left, scoped text right, StringComparison comparison)
    {
        return MemoryExtensions.Equals(left, right, comparison);
    }

    public static bool Equate(scoped text left, scoped text right, IEqualityComparer<char>? charComparer)
    {
        return MemoryExtensions.SequenceEqual<char>(left, right, charComparer);
    }

    public static bool Equate(scoped text left, scoped text right, IEqualityComparer<string>? stringComparer)
    {
        if (stringComparer is not null)
        {
            return stringComparer.Equals(left.ToString(), right.ToString());
        }
        else
        {
            return MemoryExtensions.Equals(left, right, StringComparison.Ordinal);
        }
    }

#if NET9_0_OR_GREATER
    public static bool Equate(
        scoped text left,
        scoped text right,
        IEqualityComparer<text>? stringComparer)
    {
        if (stringComparer is not null)
        {
            return stringComparer.Equals(left, right);
        }
        else
        {
            return MemoryExtensions.Equals(left, right, StringComparison.Ordinal);
        }
    }
#endif
#endregion
}