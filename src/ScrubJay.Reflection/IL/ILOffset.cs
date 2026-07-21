namespace ScrubJay.Reflection.IL;

/// <summary>
/// Represents an offset in IL
/// </summary>
[PublicAPI]
[StructLayout(LayoutKind.Explicit, Size = 4)]
public readonly struct ILOffset :
#if NET7_0_OR_GREATER
    IEqualityOperators<ILOffset, ILOffset, bool>,
    IComparisonOperators<ILOffset, ILOffset, bool>,
    IAdditionOperators<ILOffset, int, ILOffset>,
    ISubtractionOperators<ILOffset, int, ILOffset>,
#endif
    IEquatable<ILOffset>,
    IComparable<ILOffset>
{
    public static implicit operator ILOffset(int offset) => new(offset);

    public static bool operator ==(ILOffset left, ILOffset right) => left.Equals(right);
    public static bool operator !=(ILOffset left, ILOffset right) => !left.Equals(right);
    
    public static bool operator >(ILOffset left, ILOffset right) => left.CompareTo(right) > 0;
    public static bool operator >=(ILOffset left, ILOffset right) => left.CompareTo(right) >= 0;
    public static bool operator <(ILOffset left, ILOffset right) => left.CompareTo(right) < 0;
    public static bool operator <=(ILOffset left, ILOffset right) => left.CompareTo(right) <= 0;

    public static ILOffset operator +(ILOffset offset, int amount)
    {
        if (offset.IsUnknown || amount < 0)
            return Unknown;
        
        long loffset = (long)offset._offset + (long)amount;
        
        if (loffset is >= 0L and <= (long)int.MaxValue)
            return new ILOffset((int)loffset);
        
        return Unknown;
    }
    
    public static ILOffset operator -(ILOffset offset, int amount)
    {
        if (offset.IsUnknown || amount < 0)
            return Unknown;
        
        long loffset = (long)offset._offset - (long)amount;
        
        if (loffset is >= 0L and <= (long)int.MaxValue)
            return new ILOffset((int)loffset);
        
        return Unknown;
    }
    
    public static readonly ILOffset Unknown = new ILOffset(-1);
    
    
    
    [FieldOffset(0)]
    private readonly int _offset;

    public bool IsUnknown
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _offset < 0;
    }
    
    public ILOffset(int offset)
    {
        _offset = offset < 0 ? -1 : offset;
    }

    public int CompareTo(ILOffset other)
    {
        // Unknown < Known
        
        if (IsUnknown)
        {
            if (other.IsUnknown)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }
        else
        {
            if (other.IsUnknown)
            {
                return 1;
            }
            else
            {
                return _offset.CompareTo(other._offset);
            }
        }
    }

    public bool Equals(ILOffset other)
    {
        if (IsUnknown)
            return other.IsUnknown;
        
        if (other.IsUnknown)
            return false;
        
        return _offset.Equals(other._offset);
    }

    public bool Equals(int offset)
    {
        if (IsUnknown)
            return offset < 0;
        
        if (offset < 0)
            return false;
        
        return _offset.Equals(offset);
    }

    public override bool Equals([NotNullWhen(true)] object? obj) => obj switch
    {
        ILOffset ilOffset => Equals(ilOffset),
        int offset => Equals(offset),
        _ => false,
    };

    public override int GetHashCode() => _offset;

    public override string ToString()
    {
        int offset = _offset;
        if (offset < 0)
        {
            return "IL_??";
        }
        else if (offset < ushort.MaxValue)
        {
            return $"IL_{offset:X4}";
        }
        else
        {
            return $"IL_{offset:X8}";
        }
    }
}