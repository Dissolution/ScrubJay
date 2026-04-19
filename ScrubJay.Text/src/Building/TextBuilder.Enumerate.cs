namespace ScrubJay.Text.Building;

public partial class TextBuilder
{
    /* Enumeration targets should include:
     * - scoped ReadOnlySpan<T> span
     * - T[]? array
     * - IEnumerable<T>? enumerable
     * - Func<Option<T>> iterator
     */

#region Enumerate Write
    public TextBuilder Enumerate<T>(scoped ReadOnlySpan<T> span)
    {
        foreach (T item in span)
        {
            Write<T>(item);
        }

        return this;
    }

    public TextBuilder Enumerate<T>(T[]? array)
    {
        if (array is not null)
        {
            foreach (T item in array)
            {
                Write<T>(item);
            }
        }

        return this;
    }

    public TextBuilder Enumerate<T>(IEnumerable<T>? values)
    {
        if (values is not null)
        {
            foreach (var value in values)
            {
                Write<T>(value);
            }
        }

        return this;
    }

#if NET9_0_OR_GREATER
    public TextBuilder Enumerate<T>(IEnumerable<T>? values, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        if (values is not null)
        {
            foreach (var value in values)
            {
                Write<T>(value, _);
            }
        }

        return this;
    }
#endif

    public TextBuilder Enumerate<T>(Func<Option<T>>? iterator)
    {
        if (iterator is not null)
        {
            while (iterator().IsSome(out var nextItem))
            {
                Write<T>(nextItem);
            }
        }

        return this;
    }

#if NET9_0_OR_GREATER
    public TextBuilder Enumerate<T>(Func<RefOption<T>>? iterator, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        if (iterator is not null)
        {
            while (iterator().IsSome(out var nextItem))
            {
                Write<T>(nextItem, _);
            }
        }

        return this;
    }
#endif
#endregion

#region Enumerate Build
    public TextBuilder Enumerate<T>(scoped ReadOnlySpan<T> span, Action<TextBuilder, T>? itemBuild)
    {
        if (itemBuild is null) 
            return Enumerate<T>(span);

        foreach (var value in span)
        {
            itemBuild(this, value);
        }
        return this;
    }

    public TextBuilder Enumerate<T>(T[]? array, Action<TextBuilder, T>? itemBuild)
    {
        if (array.IsNullOrEmpty()) return this;
        if (itemBuild is null) return Enumerate<T>(array);
        
        foreach (var value in array)
        {
            itemBuild(this, value);
        }

        return this;
    }

    public TextBuilder Enumerate<T>(IEnumerable<T>? values, Action<TextBuilder, T>? itemBuild)
    {
        if (values is null) return this;
        if (itemBuild is null) return Enumerate<T>(values);
       
        foreach (var value in values)
        {
            itemBuild(this, value);
        }

        return this;
    }

#if NET9_0_OR_GREATER
    public TextBuilder Enumerate<T>(IEnumerable<T>? values, Action<TextBuilder, T>? itemBuild, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        if (values is null) return this;
        if (itemBuild is null) return Enumerate<T>(values, _);
        
        foreach (var value in values)
        {
            itemBuild(this, value);
        }

        return this;
    }
#endif

    public TextBuilder Enumerate<T>(Func<Option<T>>? iterator, Action<TextBuilder, T>? itemBuild)
    {
        if (iterator is null) return this;
        if (itemBuild is null) return Enumerate<T>(iterator);
        
        while (iterator().IsSome(out var nextItem))
        {
            itemBuild(this, nextItem);
        }

        return this;
    }

#if NET9_0_OR_GREATER
    public TextBuilder Enumerate<T>(Func<RefOption<T>>? iterator, Action<TextBuilder, T>? itemBuild, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        if (iterator is null) return this;
        if (itemBuild is null) return Enumerate<T>(iterator, _);
        
        while (iterator().IsSome(out var nextItem))
        {
            itemBuild(this, nextItem);
        }

        return this;
    }
#endif
#endregion

#region Enumerate w/Index
    public TextBuilder Enumerate<T>(scoped ReadOnlySpan<T> values, Action<TextBuilder, T, int>? itemIndexBuild)
    {
        if (itemIndexBuild is null) return Enumerate<T>(values);
        
        for (int i = 0; i < values.Length; i++)
        {
            itemIndexBuild(this, values[i], i);
        }

        return this;
    }

    public TextBuilder Enumerate<T>(T[]? values, Action<TextBuilder, T, int>? itemIndexBuild)
    {
        if (values is null) return this;
        if (itemIndexBuild is null) return Enumerate<T>(values);
        
        for (int i = 0; i < values.Length; i++)
        {
            itemIndexBuild(this, values[i], i);
        }
        return this;
    }

    public TextBuilder Enumerate<T>(IEnumerable<T>? values, Action<TextBuilder, T, int>? itemIndexBuild)
    {
        if (values is null) return this;
        if (itemIndexBuild is null) return Enumerate<T>(values);
        
        if (values is IList<T> list)
        {
            for (var i = 0; i < list.Count; i++)
            {
                itemIndexBuild(this, list[i], i);
            }
        }
        else
        {
            int i = 0;
            foreach (var value in values)
            {
                itemIndexBuild(this, value, i);
                i++;
            }
        }

        return this;
    }

#if NET9_0_OR_GREATER
    public TextBuilder Enumerate<T>(IEnumerable<T>? values, Action<TextBuilder, T, int>? itemIndexBuild, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        if (values is null) return this;
        if (itemIndexBuild is null) return Enumerate<T>(values);
        
        int i = 0;
        foreach (var value in values)
        {
            itemIndexBuild(this, value, i);
            i++;
        }

        return this;
    }
#endif

    public TextBuilder Enumerate<T>(Func<Option<T>>? iterator, Action<TextBuilder, T, int>? itemIndexBuild)
    {
        if (iterator is null) return this;
        if (itemIndexBuild is null) return Enumerate<T>(iterator);
        
        int i = 0;
        while (iterator().IsSome(out var nextItem))
        {
            itemIndexBuild(this, nextItem, i);
            i++;
        }

        return this;
    }

#if NET9_0_OR_GREATER
    public TextBuilder Enumerate<T>(Func<RefOption<T>>? iterator, Action<TextBuilder, T, int>? itemIndexBuild, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        if (iterator is null) return this;
        if (itemIndexBuild is null) return Enumerate<T>(iterator, _);
        
        int i = 0;
        while (iterator().IsSome(out var nextItem))
        {
            itemIndexBuild(this, nextItem, i);
            i++;
        }

        return this;
    }
#endif
#endregion
    
}