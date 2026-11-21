using System.Collections.Immutable;
using System.Reflection;
using System.Reflection.Emit;
using ScrubJay.Collections;
using ScrubJay.Reflection;
using ImmutableArray = ScrubJay.Collections.ImmutableArray;

namespace ScrubJay.Debugging;

public sealed record class DumpNode
{
    public DumpNode? Parent { get; init; }
    public View<DumpNode> Children { get; init; }

    public string? Name { get; init; }
    public Type? Type { get; init; }
    public string? Display { get; init; }
}

public static class Dump
{


    private sealed class Dumper
    {
        // keep track of already dumped objects so we don't infinitely cycle.
        private readonly HashSet<object> _dumped = new();
    }
    
  
}

public sealed class DeepObjectEqualityComparer : IEqualityComparer<object>, IEqualityComparer
{
    private static readonly ConcurrentTypeDictionary<Func<object?, object?, bool>> _cache = [];

    private static Func<object?, object?, bool> CreateEquate(Type type)
    {
        // see if Type has a Equals(self) method
        var method = 
    }
    
    private static Func<object?, object?, bool> GetEquate(Type type)
    {
        return _cache.GetOrAdd(type, CreateEquate);
    }
}



[PublicAPI]
public ref struct Dumper : IDisposable
{
    private sealed class MixComparer : IEqualityComparer<object>, IEqualityComparer
    {
        public static readonly MixComparer Default = new();

        bool IEqualityComparer.Equals(object? x, object? y)
        {
            if (x is not null)
            {
                return x.Equals(y);
            }
            else if (y is not null)
            {
                return y.Equals(x);
            }
            else
            {
                return true;
            }
        }

        bool IEqualityComparer<object>.Equals(object? x, object? y)
        {
            if (x is not null)
            {
                return x.Equals(y);
            }
            else if (y is not null)
            {
                return y.Equals(x);
            }
            else
            {
                return true;
            }
        }

        int IEqualityComparer.GetHashCode(object obj)
        {
            return HashCode.Combine(obj.GetType(), obj);
        }

        int IEqualityComparer<object>.GetHashCode(object obj)
        {
            return HashCode.Combine(obj.GetType(), obj);
        }
    }

    private readonly HashSet<object> _dumped = new HashSet<object>(MixComparer.Default);
    private DefaultInterpolatedStringHandler _builder = new(1024, 0);
    private int _indent = 0;

    public Dumper()
    {
    }

    private void DumpArray(Array array)
    {
        Debug.Assert(array is not null);
        int rank = array!.Rank;
        if (rank == 1)
        {
            var lb = array.GetLowerBound(0);
            var ub = array.GetUpperBound(0);
            if (ub < 0)
            {
                _builder.AppendLiteral("[]");
            }
            else
            {
                _builder.AppendLiteral("[");
                int i = lb;
                DumpObject(array.GetValue(i));
                for (i++; i <= ub; i++)
                {
                    _builder.AppendLiteral(", ");
                    DumpObject(array.GetValue(i));
                }
                _builder.AppendLiteral("]");
            }
        }
        else
        {
            throw new NotImplementedException();
        }
    }


    private void DumpObject(object? value)
    {
        if (value is null)
        {
            _builder.AppendLiteral("null");
            return;
        }

        if (!_dumped.Add(value))
        {
            // do not dump again - prevents infinite recursion
            _builder.AppendLiteral("<already dumped>");
        }

        if (value is bool boolean)
        {
            _builder.AppendLiteral(boolean ? "true" : "false");
        }
        else if (value is byte u8)
        {
            _builder.AppendFormatted(u8, "D");
        }
        else if (value is sbyte i8)
        {
            _builder.AppendFormatted(i8, "D");
        }
        else if (value is short i16)
        {
            _builder.AppendFormatted(i16, "D");
        }
        else if (value is ushort u16)
        {
            _builder.AppendFormatted(u16, "D");
        }
        else if (value is int i32)
        {
            _builder.AppendFormatted(i32, "D");
        }
        else if (value is uint u32)
        {
            _builder.AppendFormatted(u32, "D");
            _builder.AppendLiteral("U");
        }
        else if (value is long i64)
        {
            _builder.AppendFormatted(i64, "D");
            _builder.AppendLiteral("L");
        }
        else if (value is ulong u64)
        {
            _builder.AppendFormatted(u64, "D");
            _builder.AppendLiteral("UL");
        }
        else if (value is nint nativeInt)
        {
            _builder.AppendFormatted(nativeInt, "D");
        }
        else if (value is nuint nativeUInt)
        {
            _builder.AppendFormatted(nativeUInt, "D");
        }
        else if (value is BigInteger bigInt)
        {
            _builder.AppendFormatted(bigInt, "R");
        }
#if NET6_0_OR_GREATER
        else if (value is Half f16)
        {
            _builder.AppendFormatted(f16, "R");
        }
#endif
        else if (value is float f32)
        {
            _builder.AppendFormatted(f32, "G9");
            _builder.AppendLiteral("f");
        }
        else if (value is double f64)
        {
            _builder.AppendFormatted(f64, "G17");
            _builder.AppendLiteral("d");
        }
        else if (value is decimal dec)
        {
            _builder.AppendFormatted(dec); // default is round-trippable
            _builder.AppendLiteral("m");
        }
        else if (value is TimeSpan timeSpan)
        {
            _builder.AppendFormatted(timeSpan, "c");
        }
#if NET6_0_OR_GREATER
        else if (value is TimeOnly time)
        {
            _builder.AppendFormatted(time, "O");
        }
        else if (value is DateOnly date)
        {
            _builder.AppendFormatted(date, "O");
        }
#endif
        else if (value is DateTime dateTime)
        {
            _builder.AppendFormatted(dateTime, "O");
        }
        else if (value is DateTimeOffset dateTimeOffset)
        {
            _builder.AppendFormatted(dateTimeOffset, "O");
        }
        else if (value is Guid guid)
        {
            _builder.AppendFormatted(guid, "D");
        }
        else if (value is char ch)
        {
            _builder.AppendLiteral("'");
            _builder.AppendFormatted(ch);
            _builder.AppendLiteral("'");
        }
        else if (value is string str)
        {
            _builder.AppendLiteral("\"");
            _builder.AppendLiteral(str);
            _builder.AppendLiteral("\"");
        }
#if !NETSTANDARD2_0
        else if (value is ITuple tuple)
        {
            _builder.AppendLiteral("(");
            int c = tuple.Length;
            if (c > 0)
            {
                DumpObject(tuple[0]);
                for (var i = 1; i < c; i++)
                {
                    _builder.AppendLiteral(", ");
                    DumpObject(tuple[i]);
                }
            }
            _builder.AppendLiteral(")");
        }
#endif
        else if (value is Array array)
        {
            DumpArray(array);
        }
        else
        {
            // fallback to complex object destructure
            _builder.AppendLiteral("ToString=\"");
            _builder.AppendFormatted(value.ToString());
            _builder.AppendLiteral("\"");

            var properties = value.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (properties.Length > 0)
            {
                _indent++;
                foreach (var property in properties)
                {
                    DumpProperty(property, value);
                }
                _indent--;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }

    private void DumpProperty(PropertyInfo property, object? instance)
    {
        // start a new line
        _builder.AppendLiteral(Environment.NewLine);

        // indent
        _builder.AppendFormatted(new string(' ', _indent * 2));

        // name : type = value
        _builder.AppendLiteral(property.Name);
        _builder.AppendLiteral(" : ");
        _builder.AppendLiteral(property.PropertyType.Alias);
        _builder.AppendLiteral(" = ");

        object? value;
        try
        {
            value = property.GetValue(instance);
        }
        catch (Exception ex)
        {
            value = ex.Message;
        }

        DumpObject(value);
    }

    internal void Start<T>(T? value, string? name = null)
    {
        if (!string.IsNullOrEmpty(name))
        {
            _builder.AppendLiteral(name!);
            _builder.AppendLiteral(" : ");
        }

        _builder.AppendLiteral(TypeAlias.For<T>(in value));
        _builder.AppendLiteral(" = ");

        DumpObject((object?)value);
    }

    [HandlesResourceDisposal]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        _builder.Clear();
    }

    [HandlesResourceDisposal]
    public string ToStringAndDispose()
    {
        string str = this.ToString();
        this.Dispose();
        return str;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() => _builder.ToString();
}