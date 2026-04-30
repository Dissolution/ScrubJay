using ScrubJay.Universal.Extensions;
// ReSharper disable MethodOverloadWithOptionalParameter

namespace ScrubJay.Text.Building;

public partial class TextBuilder
{
#region If(predicate)
    public TextBuilder If(bool condition,
        Action<TextBuilder>? onTrue = null,
        Action<TextBuilder>? onFalse = null)
    {
        if (condition)
        {
            onTrue?.Invoke(this);
        }
        else
        {
            onFalse?.Invoke(this);
        }
        return this;
    }

    public TextBuilder If(Func<bool> condition,
        Action<TextBuilder>? onTrue = null,
        Action<TextBuilder>? onFalse = null)
    {
        if (condition())
        {
            onTrue?.Invoke(this);
        }
        else
        {
            onFalse?.Invoke(this);
        }
        return this;
    }
#endregion

#region If(value, valuePredicate)
    public TextBuilder If<T>(T? value, Func<T?, bool>? predicate)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        if (predicate is not null && predicate.Invoke(value))
        {
            Write<T>(value);
        }
        return this;
    }


    public TextBuilder If<T>(T value,
        Func<T, bool>? predicate,
        Action<TextBuilder, T>? onTrue = null,
        Action<TextBuilder, T>? onFalse = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        if (predicate is not null)
        {
            if (predicate.Invoke(value))
            {
                onTrue?.Invoke(this, value);
            }
            else
            {
                onFalse?.Invoke(this, value);
            }
        }
        return this;
    }
#endregion

#region IfSelect
    public TextBuilder IfSelect<T, N>(T value, Func<T, Option<N>> selectWhere)
    {
        if (selectWhere(value).IsSome(out var selected))
        {
            Write<N>(selected);
        }
        return this;
    }

    public TextBuilder IfSelect<T, N>(
        T value,
        Func<T, Option<N>> selectWhere,
        Action<TextBuilder, N>? onSelected = null,
        Action<TextBuilder, T>? onUnselected = null)
    {
        if (selectWhere(value).IsSome(out var selected))
        {
            onSelected?.Invoke(this, selected);
        }
        else
        {
            onUnselected?.Invoke(this, value);
        }
        return this;
    }

    public TextBuilder IfSelect<T, N>(
        T value,
        Func<T, Result<N>> selectWhere)
    {
        if (selectWhere(value).IsOk(out var selected))
        {
            Write<N>(selected);
        }
        return this;
    }

    public TextBuilder IfSelect<T, N>(
        T value,
        Func<T, Result<N>> selectWhere,
        Action<TextBuilder, N>? onSelected = null,
        Action<TextBuilder, T>? onUnselected = null)
    {
        if (selectWhere(value).IsOk(out var selected))
        {
            onSelected?.Invoke(this, selected);
        }
        else
        {
            onUnselected?.Invoke(this, value);
        }
        return this;
    }
#endregion

#region IfNotNull
    public TextBuilder IfNotNull<T>(T? value)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        if (value is not null)
        {
            Write<T>(value);
        }
        return this;
    }

    public TextBuilder IfNotNull<T>(
        T? value,
        Action<TextBuilder, T>? onNotNull = null,
        Action<TextBuilder>? onNull = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        if (value is not null)
        {
            onNotNull?.Invoke(this, value);
        }
        else
        {
            onNull?.Invoke(this);
        }
        return this;
    }

    public TextBuilder IfNotNull<N>(Nullable<N> nullable)
        where N : struct
    {
        if (nullable.TryGetValue(out var n))
        {
            Write<N>(n);
        }
        return this;
    }

    public TextBuilder IfNotNull<N>(Nullable<N> nullable,
        Action<TextBuilder, N>? onNotNull = null,
        Action<TextBuilder>? onNull = null)
        where N : struct
    {
        if (nullable.TryGetValue(out var n))
        {
            onNotNull?.Invoke(this, n);
        }
        else
        {
            onNull?.Invoke(this);
        }
        return this;
    }
#endregion

#region If Not Empty
    public TextBuilder IfNotEmpty(string? str)
    {
        if (!string.IsNullOrEmpty(str))
        {
            Write(str);
        }
        return this;
    }

    public TextBuilder IfNotEmpty(
        string? str,
        Action<TextBuilder, string>? onNotEmpty = null,
        Action<TextBuilder>? onEmpty = null)
    {
        if (string.IsNullOrEmpty(str))
        {
            onEmpty?.Invoke(this);
        }
        else
        {
            onNotEmpty?.Invoke(this, str);
        }
        return this;
    }

    public TextBuilder IfNotEmpty(scoped text text)
    {
        if (!text.IsEmpty)
        {
            Write(text);
        }
        return this;
    }


    public TextBuilder IfNotEmpty(
        scoped text text,
#if NET9_0_OR_GREATER
        Action<TextBuilder, text>? onNotEmpty = null,
#else
        BuildWithReadOnlySpan<char>? onNotEmpty = null,
#endif
        Action<TextBuilder>? onEmpty = null)
    {
        if (text.IsEmpty)
        {
            onEmpty?.Invoke(this);
        }
        else
        {
            onNotEmpty?.Invoke(this, text);
        }
        return this;
    }


    public TextBuilder IfNotEmpty<T>(
        scoped ReadOnlySpan<T> span,
#if NET9_0_OR_GREATER
        Action<TextBuilder, ReadOnlySpan<T>>? onNotEmpty = null,
#else
        BuildWithReadOnlySpan<T>? onNotEmpty = null,
#endif
        Action<TextBuilder>? onEmpty = null)
    {
        if (span.IsEmpty)
        {
            onEmpty?.Invoke(this);
        }
        else
        {
            onNotEmpty?.Invoke(this, span);
        }
        return this;
    }

    public TextBuilder IfNotEmpty<T>(
        T[]? array,
        Action<TextBuilder, T[]>? onNotEmpty = null,
        Action<TextBuilder>? onEmpty = null)
    {
        if (array is null || array.Length == 0)
        {
            onEmpty?.Invoke(this);
        }
        else
        {
            onNotEmpty?.Invoke(this, array);
        }
        return this;
    }

    public TextBuilder IfNotEmpty<T>(
        ICollection<T>? collection,
        Action<TextBuilder, ICollection<T>>? onNotEmpty = null,
        Action<TextBuilder>? onEmpty = null)
    {
        if (collection is null || collection.Count == 0)
        {
            onEmpty?.Invoke(this);
        }
        else
        {
            onNotEmpty?.Invoke(this, collection);
        }
        return this;
    }

    public TextBuilder IfNotEmpty<C>(
        C? collection,
        Action<TextBuilder, C>? onNotEmpty = null,
        Action<TextBuilder>? onEmpty = null)
        where C : ICollection
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        if (collection is null || collection.Count == 0)
        {
            onEmpty?.Invoke(this);
        }
        else
        {
            onNotEmpty?.Invoke(this, collection);
        }
        return this;
    }
#endregion
    
#region IfSome
    public TextBuilder IfSome<T>(Option<T> option)
    {
        if (option.IsSome(out var some))
        {
            Write<T>(some);
        }
        return this;
    }

    public TextBuilder IfSome<T>(
        Option<T> option,
        Action<TextBuilder, T>? onSome = null,
        Action<TextBuilder>? onNone = null)
    {
        if (option.IsSome(out var some))
        {
            onSome?.Invoke(this, some);
        }
        else
        {
            onNone?.Invoke(this);
        }
        return this;
    }

    public TextBuilder IfSome<T>(RefOption<T> option)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        if (option.IsSome(out var some))
        {
            Write<T>(some);
        }
        return this;
    }

    public TextBuilder IfSome<T>(
        RefOption<T> option,
        Action<TextBuilder, T>? onSome = null,
        Action<TextBuilder>? onNone = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        if (option.IsSome(out var some))
        {
            onSome?.Invoke(this, some);
        }
        else
        {
            onNone?.Invoke(this);
        }
        return this;
    }
#endregion

#region IfOk
    public TextBuilder IfOk(
        Result result,
        Action<TextBuilder>? onOk = null,
        Action<TextBuilder, Exception>? onError = null)
    {
        if (result.IsError(out var error))
        {
            onError?.Invoke(this, error);
        }
        else
        {
            onOk?.Invoke(this);
        }
        return this;
    }

    public TextBuilder IfOk(
        Result result,
        Action<TextBuilder, Unit>? onOk = null,
        Action<TextBuilder, Exception>? onError = null)
    {
        if (result.IsError(out var error))
        {
            onError?.Invoke(this, error);
        }
        else
        {
            onOk?.Invoke(this, default(Unit));
        }
        return this;
    }

    public TextBuilder IfOk<T>(Result<T> result)
    {
        if (result.IsOk(out var ok))
        {
            Write<T>(ok);
        }
        return this;
    }

    public TextBuilder IfOk<T>(
        Result<T> result,
        Action<TextBuilder, T>? onOk = null,
        Action<TextBuilder, Exception>? onError = null)
    {
        if (result.IsOk(out var ok, out var error))
        {
            onOk?.Invoke(this, ok);
        }
        else
        {
            onError?.Invoke(this, error);
        }
        return this;
    }


    public TextBuilder IfOk<T, E>(Result<T, E> result)
    {
        if (result.IsOk(out var ok))
        {
            Write<T>(ok);
        }
        return this;
    }

    public TextBuilder IfOk<T, E>(
        Result<T, E> result,
        Action<TextBuilder, T>? onOk = null,
        Action<TextBuilder, E>? onError = null)
    {
        if (result.IsOk(out var ok, out var error))
        {
            onOk?.Invoke(this, ok);
        }
        else
        {
            onError?.Invoke(this, error);
        }
        return this;
    }

    public TextBuilder IfOk<T>(RefResult<T> result)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        if (result.IsOk(out var ok))
        {
            Write<T>(ok);
        }
        return this;
    }

    public TextBuilder IfOk<T>(
        RefResult<T> result,
        Action<TextBuilder, T>? onOk = null,
        Action<TextBuilder, Exception>? onError = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        if (result.IsOk(out var ok, out var error))
        {
            onOk?.Invoke(this, ok);
        }
        else
        {
            onError?.Invoke(this, error);
        }
        return this;
    }
#endregion

}