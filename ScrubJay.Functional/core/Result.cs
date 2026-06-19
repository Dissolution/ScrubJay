using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace ScrubJay.Functional;

[PublicAPI]
[Union]
public readonly struct Result<T, E> : IUnion
{
    private readonly bool _isOk;
    private readonly T? _value;
    private readonly E? _error;
    
    internal bool HasValue
    {
        get
        {
            Debugger.Break();
            return _isOk ? _value is not null : _error is not null;
        }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public bool TryGetValue([MaybeNullWhen(false)] out T value)
    {
        Debugger.Break();
        
        if (_isOk)
        {
            value = _value!;
            return true;
        }
        value = default;
        return false;
    }
    
    [EditorBrowsable(EditorBrowsableState.Never)]
    public bool TryGetValue([MaybeNullWhen(false)] out E error)
    {
        Debugger.Break();
        
        if (!_isOk)
        {
            error = _error!;
            return true;
        }

        error = default;
        return false;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public object? Value
    {
        get
        {
            Debugger.Break();
            return _isOk ? _value : _error;
        }
    }

    public Result(T value)
    {
        _isOk = true;
        _value = value;
        _error = default;
    }

    public Result(E error)
    {
        _isOk = false;
        _value = default;
        _error = error;
    }

    public bool IsOk() => _isOk;

    public bool IsOk([MaybeNullWhen(false)] out T value)
    {
        value = _value;
        return _isOk;
    }

    public bool IsOk([MaybeNullWhen(false)] out T value, [MaybeNullWhen(true)] out E error)
    {
        value = _value;
        error = _error;
        return _isOk;
    }

    public bool IsError() => !_isOk;

    public bool IsError([MaybeNullWhen(false)] out E error)
    {
        error = _error;
        return !_isOk;
    }

    public bool IsError([MaybeNullWhen(false)] out E error, [MaybeNullWhen(true)] out T value)
    {
        error = _error;
        value = _value;
        return !_isOk;
    }


    [Obsolete]
    [DoesNotReturn]
    public override bool Equals([NotNullWhen(true)] object? obj) => throw new InvalidOperationException();
//
//    public override int GetHashCode() => base.GetHashCode();
//
//    public override string ToString() => base.ToString();
}