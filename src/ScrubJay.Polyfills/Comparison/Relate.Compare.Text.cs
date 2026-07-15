// ReSharper disable InvokeAsExtensionMember

namespace ScrubJay.Polyfills.Comparison;

public static partial class Relate
{
#region Compare(char, char, ?)
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Compare(char left, char right)
    {
        return (int)left - (int)right;
    }

    public static int Compare(char left, char right, StringComparison comparison)
    {
        if (comparison != StringComparison.Ordinal)
        {
            return Compare(left.AsSpan(), right.AsSpan(), comparison);
        }
        else
        {
            return Compare(left, right);
        }
    }

    public static int Compare(char left, char right, IComparer<char>? charComparer)
    {
        if (charComparer is not null)
        {
            return charComparer.Compare(left, right);
        }
        else
        {
            return Compare(left, right);
        }
    }

    public static int Compare(char left, char right, IComparer<string>? stringComparer)
    {
        if (stringComparer is not null)
        {
            return stringComparer.Compare(left.ToString(), right.ToString());
        }
        else
        {
            return Compare(left, right);
        }
    }

#if NET9_0_OR_GREATER
    public static int Compare(char left, char right, IComparer<text>? stringComparer)
    {
        if (stringComparer is not null)
        {
            return stringComparer.Compare(left.AsSpan(), right.AsSpan());
        }
        else
        {
            return Compare(left, right);
        }
    }
#endif
#endregion
    
#region Compare(text, text, ?)
    public static int Compare(scoped text left, scoped text right)
    {
        return MemoryExtensions.CompareTo(left, right, StringComparison.Ordinal);
    }

    public static int Compare(scoped text left, scoped text right, StringComparison comparison)
    {
        return MemoryExtensions.CompareTo(left, right, comparison);
    }

    public static int Compare(scoped text left, scoped text right, IComparer<char>? charComparer)
    {
#if NET10_0_OR_GREATER
        return MemoryExtensions.SequenceCompareTo<char>(left, right, charComparer);
#else
        if (charComparer is not null)
        {
            int minLength = Math.Min(left.Length, right.Length);
            for (int i = 0; i < minLength; i++)
            {
                int c = charComparer.Compare(left[i], right[i]);
                if (c != 0)
                {
                    return c;
                }
            }
        }
        else
        {
            int minLength = Math.Min(left.Length, right.Length);
            for (int i = 0; i < minLength; i++)
            {
                int c = Compare(left[i], right[i]);
                if (c != 0)
                {
                    return c;
                }
            }
        }
        
        return left.Length.CompareTo(right.Length);
#endif
    }

    public static int Compare(scoped text left, scoped text right, IComparer<string>? stringComparer)
    {
        if (stringComparer is not null)
        {
            return stringComparer.Compare(left.ToString(), right.ToString());
        }
        else
        {
            return Compare(left, right);
        }
    }

#if NET9_0_OR_GREATER
    public static int Compare(
        scoped text left,
        scoped text right,
        IComparer<text>? textComparer)
    {
        if (textComparer is not null)
        {
            return textComparer.Compare(left, right);
        }
        else
        {
            return Compare(left, right);
        }
    }
#endif
#endregion
}