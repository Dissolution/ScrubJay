using System.Reflection;
using ScrubJay.Rendering.Rendition5;

namespace ScrubJay.Dumping;

[PublicAPI]
public static class Dump
{
    public static Func<DumpNode, bool> SkipNullValues { get; } = static box => box.ContainsNull;

    public static Func<DumpNode, bool> SkipDefaultValues { get; } = static box =>
    {
        if (box.Type.IsValueType && !box.Type.IsNullable())
        {
            return Equals(box.Object, Activator.CreateInstance(box.Type));
        }
        else
        {
            return box.ContainsNull;
        }
    };

}

public class Dumper
{
    public List<Func<DumpNode, bool>> Skips { get; } = [];

    public Dumper()
    {

    }

    private StringBuilder DumpArray(Type type, Array array, StringBuilder builder, int indent)
    {
        Debug.Assert(type.IsArray);

        var elementType = type.GetElementType()!;

        int rank = array.Rank;

        if (rank == 1)
        {
            builder.Append($"{{ Count = {array.Length} }} [");

            bool wrote = false;
            foreach (object? obj in array)
            {
                var node = new DumpNode { Name = null, Type = elementType, Object = obj };
                if (TryDumpNode(node, builder, indent + 1))
                    wrote = true;
            }

            if (wrote)
            {
                builder.AppendLine().Append(' ', indent * 2);
            }

            return builder.Append(']');
        }
        else
        {
            throw Ex.NotImplemented();
        }
    }

    private StringBuilder DumpDictionary(Type type, IDictionary dictionary, StringBuilder builder, int indent)
    {
        Debug.Assert(type.IsAssignableTo<IDictionary>());

        var genericTypes = type.GetGenericArguments();
        Type keyType;
        Type valueType;
        if (genericTypes.Length == 2)
        {
            keyType = genericTypes[0];
            valueType = genericTypes[1];
            builder.Append($"IDictionary<{keyType.Render()}, {valueType.Render()}> {{ Count = {dictionary.Count} }} {{");
        }
        else
        {
            keyType = typeof(object);
            valueType = typeof(object);
            builder.Append($"IDictionary {{ Count = {dictionary.Count} }} {{");
        }

        bool wrote = false;
        foreach (DictionaryEntry entry in dictionary)
        {
            var valueNode = new DumpNode { Name = null, Type = valueType, Object = entry.Value };
            if (Skips.Any(skip => skip(valueNode)))
                continue;
            var keyNode = new DumpNode { Name = null, Type = keyType, Object = entry.Key };
            DumpValue(keyNode, builder, indent + 1);
            builder.Append(": ");
            DumpValue(valueNode, builder, indent + 1);
            wrote = true;
        }

        if (wrote)
        {
            builder.AppendLine().Append(' ', indent * 2);
        }

        return builder.Append('}');
    }

    private StringBuilder DumpList(Type type, IList list, StringBuilder builder, int indent)
    {
        Debug.Assert(type.IsAssignableTo<IList>());

        var genericTypes = type.GetGenericArguments();
        Type itemType;
        if (genericTypes.Length == 1)
        {
            itemType = genericTypes[0];
            builder.Append($"IList<{itemType.Render()}> {{ Count = {list.Count} }} [");
        }
        else
        {
            itemType = typeof(object);
            builder.Append($"IList {{ Count = {list.Count} }} [");
        }

        bool wrote = false;
        foreach (object? item in list)
        {
            var itemNode = new DumpNode { Name = null, Type = itemType, Object = item };
            if (Skips.Any(skip => skip(itemNode)))
                continue;
            DumpValue(itemNode, builder, indent + 1);
            wrote = true;
        }

        if (wrote)
        {
            builder.AppendLine().Append(' ', indent * 2);
        }

        return builder.Append(']');
    }

    private StringBuilder DumpEnumerable(Type type, IEnumerable enumerable, StringBuilder builder, int indent)
    {
        Debug.Assert(type.IsAssignableTo<IEnumerable>());

        var genericTypes = type.GetGenericArguments();
        Type itemType;
        if (genericTypes.Length == 1)
        {
            itemType = genericTypes[0];
            builder.Append($"IEnumerable<{itemType.Render()}> {{");
        }
        else
        {
            itemType = typeof(object);
            builder.Append("IEnumerable {{");
        }

        bool wrote = false;
        foreach (object? item in enumerable)
        {
            var itemNode = new DumpNode { Name = null, Type = itemType, Object = item };
            if (Skips.Any(skip => skip(itemNode)))
                continue;
            DumpValue(itemNode, builder, indent + 1);
            wrote = true;
        }

        if (wrote)
        {
            builder.AppendLine().Append(' ', indent * 2);
        }

        return builder.Append('}');
    }

    private StringBuilder DumpTuple(Type type, ITuple tuple, StringBuilder builder, int indent)
    {
        var tupleTypes = type.GetGenericArguments();
        int len = tuple.Length;
        if (len != tupleTypes.Length)
        {
            Debugger.Break();
        }

        builder.Append('(');
        for (int i = 0; i < len; i++)
        {
            if (i > 0)
                builder.Append(", ");
            var node = new DumpNode { Name = null, Type = tupleTypes[i], Object = tuple[i] };
            DumpValue(node, builder, indent);
        }
        return builder.Append(')');
    }

    private StringBuilder DumpValue(DumpNode node, StringBuilder builder, int indent)
    {
        var (type, obj) = node;

        if (obj is null)
            return builder.Append("`null`");
        if (obj is bool boolean)
            return builder.Append(boolean ? "true" : "false");
        if (obj is char ch)
            return builder.Append($"'{ch}'");
        if (obj is string str)
            return builder.Append($"\"{str}\"");
        if (obj is float f32)
            return builder.Append(f32.ToString("G9"));
        if (obj is double f64)
            return builder.Append(f64.ToString("G17"));
        if (obj is BigInteger bigInt)
            return builder.Append(bigInt.ToString("R"));
        if (obj is decimal dec)
            return builder.Append(dec);
        if (obj is TimeSpan timeSpan)
            return builder.Append(timeSpan.ToString("c"));
        if (obj is DateTime dateTime)
            return builder.Append(dateTime.ToString("O"));
        if (obj is DateTimeOffset dateTimeOffset)
            return builder.Append(dateTimeOffset.ToString("O"));
#if !NETSTANDARD && !NETFRAMEWORK
        if (obj is TimeOnly time)
            return builder.Append(time.ToString("O"));
        if (obj is DateOnly date)
            return builder.Append(date.ToString("O"));
#endif
        if (obj is Type t)
            return builder.Append(t.Render());
        if (obj is Array array)
            return DumpArray(type, array, builder, indent);
        if (obj is ITuple tuple)
            return DumpTuple(type, tuple, builder, indent);

        if (type.IsEnum)
            return builder.Append(Enum.GetName(type, obj));
        if (type.IsPrimitive || type == typeof(object))
            return builder.Append(obj);

        var properties = type
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(static property => property.GetIndexParameters().Length == 0)
            .Where(static property => property.CanRead)
            .Where(static property => !property.DeclaringType.IsAssignableTo<IEnumerable>())
            .ToList();

        if (properties.Count > 0)
        {
            builder.Append($"\"{obj}\" {{");

            bool wrote = false;
            foreach (var property in properties)
            {
                object? propertyValue;
                try
                {
                    propertyValue = property.GetValue(obj);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    continue;
                }

                var propertyNode = new DumpNode()
                {
                    Name = property.Name,
                    Type = property.PropertyType,
                    Object = propertyValue,
                };

                if (TryDumpNode(propertyNode, builder, indent + 1))
                    wrote = true;
            }

            if (wrote)
            {
                builder.AppendLine().Append(' ', indent * 2);
            }

            return builder.Append('}');
        }

        if (obj is IDictionary dictionary)
            return DumpDictionary(type, dictionary, builder, indent);

        if (obj is IList list)
            return DumpList(type, list, builder, indent);

        if (obj is IEnumerable enumerable)
            return DumpEnumerable(type, enumerable, builder, indent);

        Debugger.Break();
        return builder.Append(obj);
    }


    private bool TryDumpNode(DumpNode node, StringBuilder builder, int indent)
    {
        if (Skips.Any(skip => skip(node)))
            return false;

        // if there is an indent we need to start nested on a new line
        if (indent > 0)
        {
            builder.AppendLine().Append(' ', indent * 2);
        }

        var (name, type, obj) = node;

        if (string.IsNullOrEmpty(name))
        {
            // no append for blank name, used to display type only
        }
        else
        {
            builder.Append(name).Append(": ");
        }

        if (obj is null)
        {
            builder.Append("`null`");
            return true;
        }

        builder.Append(type.Render()).Append(" = ");
        DumpValue(node, builder, indent);
        return true;
    }

    public string Dump<T>(T? value, [CallerArgumentExpression(nameof(value))] string? valueName = null)
    {
        var node = DumpNode.Create<T>(value, valueName);
        var builder = new StringBuilder();
        TryDumpNode(node, builder, 0);
        return builder.ToString();
    }
}

public sealed record class DumpNode
{
    public static DumpNode Create<T>(T? value, [CallerArgumentExpression(nameof(value))] string? valueName = null)
    {
        return new DumpNode()
        {
            Name = valueName,
            Type = value?.GetType() ?? typeof(T),
            Object = (object?)value,
        };
    }

    public string? Name { get; init; } = null;

    public required Type Type { get; init; }

    public required object? Object { get; init; } = null;

    public bool ContainsNull => Object is null;

    public void Deconstruct(out string? name, out Type type, out object? obj)
    {
        name = Name;
        type = Type;
        obj = Object;
    }

    public void Deconstruct(out Type type, out object? obj)
    {
        type = Type;
        obj = Object;
    }
}