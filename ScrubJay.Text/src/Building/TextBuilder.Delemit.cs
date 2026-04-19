namespace ScrubJay.Text.Building;

public partial class TextBuilder
{
    /* Delimiter targets should include:
     * - char ch
     * - scoped ReadOnlySpan<char> text
     * - Action<TextBuilder>? delimit
     */

    /* Enumeration targets should include:
     * - scoped ReadOnlySpan<T> span
     * - T[]? array
     * - IEnumerable<T>? enumerable
     * - Func<Option<T>> iterator
     */

    /* Invocations should include:
     * - `empty` indicates Write
     * - Action<TextBuilder,T>? buildItem
     * */


#region Write
#region ReadOnlySpan<T>
    public TextBuilder Delimit<T>(char delimiter, scoped ReadOnlySpan<T> span)
    {
        if (!span.IsEmpty)
        {
            Write<T>(span[0]);
            for (int i = 1; i < span.Length; i++)
            {
                Write(delimiter);
                Write<T>(span[i]);
            }
        }
        return this;
    }

    public TextBuilder Delimit<T>(scoped text delimiter, scoped ReadOnlySpan<T> span)
    {
        if (!span.IsEmpty)
        {
            if (!delimiter.IsEmpty)
            {
                Write<T>(span[0]);
                for (int i = 1; i < span.Length; i++)
                {
                    Write(delimiter);
                    Write<T>(span[i]);
                }
                return this;
            }
            return Enumerate<T>(span);
        }
        return this;
    }

    public TextBuilder Delimit<T>(Action<TextBuilder>? delimit, scoped ReadOnlySpan<T> span)
    {
        if (!span.IsEmpty)
        {
            if (delimit is not null)
            {
                Write<T>(span[0]);
                for (int i = 1; i < span.Length; i++)
                {
                    delimit(this);
                    Write<T>(span[i]);
                }
                return this;
            }
            return Enumerate<T>(span);
        }
        return this;
    }
#endregion

#region T[]
    public TextBuilder Delimit<T>(char delimiter, T[]? array)
    {
        if (array is not null && array.Length > 0)
        {
            Write<T>(array[0]);
            for (int i = 1; i < array.Length; i++)
            {
                Write(delimiter);
                Write<T>(array[i]);
            }
        }
        return this;
    }

    public TextBuilder Delimit<T>(scoped text delimiter, T[]? array)
    {
        if (array is not null && array.Length > 0)
        {
            if (!delimiter.IsEmpty)
            {
                Write<T>(array[0]);
                for (int i = 1; i < array.Length; i++)
                {
                    Write(delimiter);
                    Write<T>(array[i]);
                }
                return this;
            }
            return Enumerate<T>(array);
        }
        return this;
    }

    public TextBuilder Delimit<T>(Action<TextBuilder>? delimit, T[]? array)
    {
        if (array is not null && array.Length > 0)
        {
            if (delimit is not null)
            {
                Write<T>(array[0]);
                for (int i = 1; i < array.Length; i++)
                {
                    delimit(this);
                    Write<T>(array[i]);
                }
                return this;
            }
            return Enumerate<T>(array);
        }
        return this;
    }
#endregion

#region IEnumerable<T>
    public TextBuilder Delimit<T>(char delimiter, IEnumerable<T>? enumerable)
    {
        if (enumerable is not null)
        {
            using var e = enumerable.GetEnumerator();
            if (!e.MoveNext())
                return this;
            Write<T>(e.Current);
            while (e.MoveNext())
            {
                Write(delimiter);
                Write<T>(e.Current);
            }
        }
        return this;
    }

    public TextBuilder Delimit<T>(scoped text delimiter, IEnumerable<T>? enumerable)
    {
        if (enumerable is not null)
        {
            if (!delimiter.IsEmpty)
            {
                using var e = enumerable.GetEnumerator();
                if (!e.MoveNext())
                    return this;
                Write<T>(e.Current);
                while (e.MoveNext())
                {
                    Write(delimiter);
                    Write<T>(e.Current);
                }
                return this;
            }
            return Enumerate<T>(enumerable);
        }
        return this;
    }

    public TextBuilder Delimit<T>(Action<TextBuilder>? delimit, IEnumerable<T>? enumerable)
    {
        if (enumerable is not null)
        {
            if (delimit is not null)
            {
                using var e = enumerable.GetEnumerator();
                if (!e.MoveNext())
                    return this;
                Write<T>(e.Current);
                while (e.MoveNext())
                {
                    delimit(this);
                    Write<T>(e.Current);
                }
                return this;
            }
            return Enumerate<T>(enumerable);
        }
        return this;
    }
#endregion

#region Func<Option<T>>? iterator
    public TextBuilder Delimit<T>(char delimiter, Func<Option<T>>? iterator)
    {
        if (iterator is not null)
        {
            if (!iterator().IsSome(out var next))
                return this;
            Write<T>(next);
            while (iterator().IsSome(out next))
            {
                Write(delimiter);
                Write<T>(next);
            }
        }
        return this;
    }

    public TextBuilder Delimit<T>(scoped text delimiter, Func<Option<T>>? iterator)
    {
        if (iterator is not null)
        {
            if (!delimiter.IsEmpty)
            {
                if (!iterator().IsSome(out var next))
                    return this;
                Write<T>(next);
                while (iterator().IsSome(out next))
                {
                    Write(delimiter);
                    Write<T>(next);
                }
                return this;
            }
            return Enumerate<T>(iterator);
        }
        return this;
    }

    public TextBuilder Delimit<T>(Action<TextBuilder>? delimit, Func<Option<T>>? iterator)
    {
        if (iterator is not null)
        {
            if (delimit is not null)
            {
                if (!iterator().IsSome(out var next))
                    return this;
                Write<T>(next);
                while (iterator().IsSome(out next))
                {
                    delimit(this);
                    Write<T>(next);
                }
                return this;
            }
            return Enumerate<T>(iterator);
        }
        return this;
    }
#endregion
#endregion

#region Build
#region ReadOnlySpan<T>
    public TextBuilder Delimit<T>(char delimiter, scoped ReadOnlySpan<T> span, Action<TextBuilder, T>? itemBuild)
    {
        if (itemBuild is null) return Delimit<T>(delimiter, span);
        if (span.IsEmpty) return this;

        Write<T>(span[0]);
        for (int i = 1; i < span.Length; i++)
        {
            Write(delimiter);
            Write<T>(span[i]);
        }
        return this;
    }

    public TextBuilder Delimit<T>(scoped text delimiter, scoped ReadOnlySpan<T> span, Action<TextBuilder, T>? itemBuild)
    {
        if (itemBuild is null) return Delimit<T>(delimiter, span);
        if (span.IsEmpty) return this;
        if (delimiter.IsEmpty) return Enumerate<T>(span, itemBuild);

        Write<T>(span[0]);
        for (int i = 1; i < span.Length; i++)
        {
            Write(delimiter);
            Write<T>(span[i]);
        }
        return this;
    }

    public TextBuilder Delimit<T>(Action<TextBuilder>? delimit, scoped ReadOnlySpan<T> span, Action<TextBuilder, T>? itemBuild)
    {
        if (itemBuild is null) return Delimit<T>(delimit, span);
        if (span.IsEmpty) return this;
        if (delimit is null) return Enumerate<T>(span, itemBuild);

        Write<T>(span[0]);
        for (int i = 1; i < span.Length; i++)
        {
            delimit(this);
            Write<T>(span[i]);
        }
        return this;
    }
#endregion

#region T[]
    public TextBuilder Delimit<T>(char delimiter, T[]? array, Action<TextBuilder, T>? itemBuild)
    {
        if (itemBuild is null) return Delimit<T>(delimiter, array);
        if (array.IsNullOrEmpty()) return this;

        Write<T>(array[0]);
        for (int i = 1; i < array.Length; i++)
        {
            Write(delimiter);
            Write<T>(array[i]);
        }
        return this;
    }

    public TextBuilder Delimit<T>(scoped text delimiter, T[]? array, Action<TextBuilder, T>? itemBuild)
    {
        if (itemBuild is null) return Delimit<T>(delimiter, array);
        if (array.IsNullOrEmpty()) return this;
        if (delimiter.IsEmpty) return Enumerate<T>(array, itemBuild);

        Write<T>(array[0]);
        for (int i = 1; i < array.Length; i++)
        {
            Write(delimiter);
            Write<T>(array[i]);
        }
        return this;
    }

    public TextBuilder Delimit<T>(Action<TextBuilder>? delimit, T[]? array, Action<TextBuilder, T>? itemBuild)
    {
        if (itemBuild is null) return Delimit<T>(delimit, array);
        if (array.IsNullOrEmpty()) return this;
        if (delimit is null) return Enumerate<T>(array, itemBuild);

        Write<T>(array[0]);
        for (int i = 1; i < array.Length; i++)
        {
            delimit(this);
            Write<T>(array[i]);
        }
        return this;
    }
#endregion

#region IEnumerable<T>
    public TextBuilder Delimit<T>(char delimiter, IEnumerable<T>? enumerable, Action<TextBuilder, T>? itemBuild)
    {
        if (itemBuild is null) return Delimit<T>(delimiter, enumerable);
        if (enumerable is null) return this;

        using var e = enumerable.GetEnumerator();
        if (!e.MoveNext())
            return this;
        Write<T>(e.Current);
        while (e.MoveNext())
        {
            Write(delimiter);
            Write<T>(e.Current);
        }

        return this;
    }

    public TextBuilder Delimit<T>(scoped text delimiter, IEnumerable<T>? enumerable, Action<TextBuilder, T>? itemBuild)
    {
        if (itemBuild is null) return Delimit<T>(delimiter, enumerable);
        if (enumerable is null) return this;
        if (delimiter.IsEmpty) return Enumerate<T>(enumerable, itemBuild);

        using var e = enumerable.GetEnumerator();
        if (!e.MoveNext())
            return this;
        Write<T>(e.Current);
        while (e.MoveNext())
        {
            Write(delimiter);
            Write<T>(e.Current);
        }

        return this;
    }

    public TextBuilder Delimit<T>(Action<TextBuilder>? delimit, IEnumerable<T>? enumerable, Action<TextBuilder, T>? itemBuild)
    {
        if (itemBuild is null) return Delimit<T>(delimit, enumerable);
        if (enumerable is null) return this;
        if (delimit is null) return Enumerate<T>(enumerable, itemBuild);

        using var e = enumerable.GetEnumerator();
        if (!e.MoveNext())
            return this;
        Write<T>(e.Current);
        while (e.MoveNext())
        {
            delimit(this);
            Write<T>(e.Current);
        }

        return this;
    }
#endregion

#region Func<Option<T>>? iterator
    public TextBuilder Delimit<T>(char delimiter, Func<Option<T>>? iterator, Action<TextBuilder, T>? itemBuild)
    {
        if (itemBuild is null) return Delimit<T>(delimiter, iterator);
        if (iterator is null) return this;

        if (!iterator().IsSome(out var next))
            return this;
        Write<T>(next);
        while (iterator().IsSome(out next))
        {
            Write(delimiter);
            Write<T>(next);
        }

        return this;
    }

    public TextBuilder Delimit<T>(scoped text delimiter, Func<Option<T>>? iterator, Action<TextBuilder, T>? itemBuild)
    {
        if (itemBuild is null) return Delimit<T>(delimiter, iterator);
        if (iterator is null) return this;
        if (delimiter.IsEmpty) return Enumerate<T>(iterator, itemBuild);

        if (!iterator().IsSome(out var next))
            return this;
        Write<T>(next);
        while (iterator().IsSome(out next))
        {
            Write(delimiter);
            Write<T>(next);
        }

        return this;
    }

    public TextBuilder Delimit<T>(Action<TextBuilder>? delimit, Func<Option<T>>? iterator, Action<TextBuilder, T>? itemBuild)
    {
        if (itemBuild is null) return Delimit<T>(delimit, iterator);
        if (iterator is null) return this;
        if (delimit is null) return Enumerate<T>(iterator, itemBuild);

        if (!iterator().IsSome(out var next))
            return this;
        Write<T>(next);
        while (iterator().IsSome(out next))
        {
            delimit(this);
            Write<T>(next);
        }

        return this;
    }
#endregion
#endregion
}