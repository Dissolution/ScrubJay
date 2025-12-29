using System.Text;

namespace ScrubJay.Interpolated;

/// <summary>
/// Gets nicer names for <see cref="Type">Types</see>!<br/>
/// <code>
/// `System.Int32` -> `int`
/// `System.Collections.Generic.IList`1` -> `IList&lt;int&gt;`
///
/// </code>
/// </summary>
[PublicAPI]
public static partial class TypeName
{
    extension(StringBuilder builder)
    {
        public StringBuilder AppendTypeName(Type? type)
        {
            return builder.Append(For(type));
        }

        public StringBuilder AppendTypeName<T>()
        {
            return builder.Append(For<T>());
        }
    }

    private static void WriteTypeName(StringBuilder builder, Type? type)
    {
        if (type is null)
        {
            builder.Append("null");
            return;
        }

        if (_typeAliases.TryGetValue(type, out var alias))
        {
            builder.Append(alias);
            return;
        }

        if (type.IsPointer)
        {
            type = type.GetElementType()!;
            WriteTypeName(builder, type);
            builder.Append('*');
            return;
        }

        if (type.IsByRef)
        {
            type = type.GetElementType()!;
            WriteTypeName(builder, type);
            builder.Append('&');
            return;
        }

        if (type.IsArray)
        {
            WriteArrayType(builder, type);
            return;
        }
    }
    
    public static string For(Type? type)
    {
        var builder = StringBuilder.Rent();
        WriteTypeName(builder, type);
        return builder.ReturnAndGetString();
    }
    
    public static string For<T>() => For(typeof(T));
}