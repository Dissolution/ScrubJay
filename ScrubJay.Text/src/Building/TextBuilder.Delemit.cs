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
     * - `empty` indicates Append (the default)
     * - Action<TextBuilder,T>? buildItem
     * */


#region Append (the default)
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
        if (!delimiter.IsEmpty)
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
        return Enumerate<T>(span);
    }

    public TextBuilder Delimit<T>(Action<TextBuilder>? delimit, scoped ReadOnlySpan<T> span)
    {
        if (delimit is not null)
        {
            if (!span.IsEmpty)
            {
                Write<T>(span[0]);
                for (int i = 1; i < span.Length; i++)
                {
                    delimit(this);
                    Write<T>(span[i]);
                }
            }
            return this;
        }
        return Enumerate<T>(span);
    }
#endregion

#region T[]
    public TextBuilder Delimit<T>(char delimiter, T[]? array)
    {
        if (!array.IsNullOrEmpty())
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
        if (!delimiter.IsEmpty)
        {
            if (!array.IsNullOrEmpty())
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
        return Enumerate<T>(array);
    }

    public TextBuilder Delimit<T>(Action<TextBuilder>? delimit, T[]? array)
    {
        if (delimit is not null)
        {
            if (!array.IsNullOrEmpty())
            {
                Write<T>(array[0]);
                for (int i = 1; i < array.Length; i++)
                {
                    delimit(this);
                    Write<T>(array[i]);
                }
            }
            return this;
        }
        return Enumerate<T>(array);
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
        if (!delimiter.IsEmpty)
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
        return Enumerate<T>(enumerable);
    }

    public TextBuilder Delimit<T>(Action<TextBuilder>? delimit, IEnumerable<T>? enumerable)
    {
        if (delimit is not null)
        {
            if (enumerable is not null)
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
            }
            return this;
        }
        return Enumerate<T>(enumerable);
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
        if (!delimiter.IsEmpty)
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
        return Enumerate<T>(iterator);
    }

    public TextBuilder Delimit<T>(Action<TextBuilder>? delimit, Func<Option<T>>? iterator)
    {
        if (delimit is not null)
        {
            if (iterator is not null)
            {
                if (!iterator().IsSome(out var next))
                    return this;
                Write<T>(next);
                while (iterator().IsSome(out next))
                {
                    delimit(this);
                    Write<T>(next);
                }
            }
            return this;
        }
        return Enumerate<T>(iterator);
    }
#endregion
#endregion

#region Build Action
#region ReadOnlySpan<T>
    public TextBuilder Delimit<T>(char delimiter, scoped ReadOnlySpan<T> span, Action<TextBuilder, T>? buildItem)
    {
        if (buildItem is not null)
        {
            if (!span.IsEmpty)
            {
                buildItem(this, span[0]);
                for (int i = 1; i < span.Length; i++)
                {
                    Write(delimiter);
                    buildItem(this, span[i]);
                }
            }
            return this;
        }
        return Delimit<T>(delimiter, span);
    }

    public TextBuilder Delimit<T>(scoped text delimiter, scoped ReadOnlySpan<T> span, Action<TextBuilder, T>? buildItem)
    {
        if (buildItem is not null)
        {
            if (!delimiter.IsEmpty)
            {
                if (!span.IsEmpty)
                {
                    buildItem(this, span[0]);
                    for (int i = 1; i < span.Length; i++)
                    {
                        Write(delimiter);
                        buildItem(this, span[i]);
                    }
                }
                return this;
            }
            return Enumerate<T>(span, buildItem);
        }
        return Delimit<T>(delimiter, span);
    }

    public TextBuilder Delimit<T>(Action<TextBuilder>? delimit, scoped ReadOnlySpan<T> span, Action<TextBuilder, T>? buildItem)
    {
        if (buildItem is not null)
        {
            if (delimit is not null)
            {
                if (!span.IsEmpty)
                {
                    buildItem(this, span[0]);
                    for (int i = 1; i < span.Length; i++)
                    {
                        delimit(this);
                        buildItem(this, span[i]);
                    }
                }
                return this;
            }
            return Enumerate<T>(span, buildItem);
        }
        return Delimit<T>(delimit, span);
    }
#endregion

#region T[]
    public TextBuilder Delimit<T>(char delimiter, T[]? array, Action<TextBuilder, T>? buildItem)
    {
        if (buildItem is not null)
        {
            if (!array.IsNullOrEmpty())
            {
                buildItem(this, array[0]);
                for (int i = 1; i < array.Length; i++)
                {
                    Write(delimiter);
                    buildItem(this, array[i]);
                }
            }
            return this;
        }
        return Delimit<T>(delimiter, array);
    }

    public TextBuilder Delimit<T>(scoped text delimiter, T[]? array, Action<TextBuilder, T>? buildItem)
    {
        if (buildItem is not null)
        {
            if (!delimiter.IsEmpty)
            {
                if (!array.IsNullOrEmpty())
                {
                    buildItem(this, array[0]);
                    for (int i = 1; i < array.Length; i++)
                    {
                        Write(delimiter);
                        buildItem(this, array[i]);
                    }
                }
                return this;
            }
            return Enumerate<T>(array, buildItem);
        }
        return Delimit<T>(delimiter, array);
    }

    public TextBuilder Delimit<T>(Action<TextBuilder>? delimit, T[]? array, Action<TextBuilder, T>? buildItem)
    {
        if (buildItem is not null)
        {
            if (delimit is not null)
            {
                if (!array.IsNullOrEmpty())
                {
                    buildItem(this, array[0]);
                    for (int i = 1; i < array.Length; i++)
                    {
                        delimit(this);
                        buildItem(this, array[i]);
                    }
                }
                return this;
            }
            return Enumerate<T>(array, buildItem);
        }
        return Delimit<T>(delimit, array);
    }
#endregion

#region IEnumerable<T>
    public TextBuilder Delimit<T>(char delimiter, IEnumerable<T>? enumerable, Action<TextBuilder, T>? buildItem)
    {
        if (buildItem is not null)
        {
            if (enumerable is not null)
            {
                using var e = enumerable.GetEnumerator();
                if (!e.MoveNext())
                    return this;
                buildItem(this, e.Current);
                while (e.MoveNext())
                {
                    Write(delimiter);
                    buildItem(this, e.Current);
                }
            }
            return this;
        }
        return Delimit<T>(delimiter, enumerable);
    }

    public TextBuilder Delimit<T>(scoped text delimiter, IEnumerable<T>? enumerable, Action<TextBuilder, T>? buildItem)
    {
        if (buildItem is not null)
        {
            if (!delimiter.IsEmpty)
            {
                if (enumerable is not null)
                {
                    using var e = enumerable.GetEnumerator();
                    if (!e.MoveNext())
                        return this;
                    buildItem(this, e.Current);
                    while (e.MoveNext())
                    {
                        Write(delimiter);
                        buildItem(this, e.Current);
                    }
                }
                return this;
            }
            return Enumerate<T>(enumerable, buildItem);
        }
        return Delimit<T>(delimiter, enumerable);
    }

    public TextBuilder Delimit<T>(Action<TextBuilder>? delimit, IEnumerable<T>? enumerable, Action<TextBuilder, T>? buildItem)
    {
        if (buildItem is not null)
        {
            if (delimit is not null)
            {
                if (enumerable is not null)
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
                }
                return this;
            }
            return Enumerate<T>(enumerable, buildItem);
        }
        return Delimit<T>(delimit, enumerable);
    }
#endregion

#region Func<Option<T>>? iterator
    public TextBuilder Delimit<T>(char delimiter, Func<Option<T>>? iterator, Action<TextBuilder, T>? buildItem)
    {
        if (buildItem is not null)
        {
            if (iterator is not null)
            {
                if (!iterator().IsSome(out var next))
                    return this;
                buildItem(this, next);
                while (iterator().IsSome(out next))
                {
                    Write(delimiter);
                    buildItem(this, next);
                }
            }
            return this;
        }
        return Delimit<T>(delimiter, iterator);
    }

    public TextBuilder Delimit<T>(scoped text delimiter, Func<Option<T>>? iterator, Action<TextBuilder, T>? buildItem)
    {
        if (buildItem is not null)
        {
            if (!delimiter.IsEmpty)
            {
                if (iterator is not null)
                {
                    if (!iterator().IsSome(out var next))
                        return this;
                    buildItem(this, next);
                    while (iterator().IsSome(out next))
                    {
                        Write(delimiter);
                        buildItem(this, next);
                    }
                }
                return this;
            }
            return Enumerate<T>(iterator, buildItem);
        }
        return Delimit<T>(delimiter, iterator);
    }

    public TextBuilder Delimit<T>(Action<TextBuilder>? delimit, Func<Option<T>>? iterator, Action<TextBuilder, T>? buildItem)
    {
        if (buildItem is not null)
        {
            if (delimit is not null)
            {
                if (iterator is not null)
                {
                    if (!iterator().IsSome(out var next))
                        return this;
                    Write<T>(next);
                    while (iterator().IsSome(out next))
                    {
                        delimit(this);
                        Write<T>(next);
                    }
                }
                return this;
            }
            return Enumerate<T>(iterator, buildItem);
        }
        return Delimit<T>(delimit, iterator);
    }
#endregion
#endregion
}