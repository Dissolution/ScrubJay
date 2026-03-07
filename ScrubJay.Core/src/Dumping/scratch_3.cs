using ScrubJay.Rendering.Rendition5;

namespace ScrubJay.Dumping;

[PublicAPI]
public static class Dump
{
    public static Func<DumpBox, bool> SkipNullValues { get; } = static box => box.ContainsNull;

    public static Func<DumpBox, bool> SkipDefaultValues { get; } = static box =>
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
    public enum SubType
    {
        Member, // {}
        Item, // []
        Value, // ()
    }

    public List<Func<DumpVar, bool>> Skips { get; } = [];

    public Dumper()
    {
        
    }


    private bool TryDumpTo(DumpBox box, StringBuilder builder, int indent)
    {
        
    }
    
    private bool TryDumpTo(DumpVar var, StringBuilder builder, int indent)
    {
        if (Skips.Any(f => f(var)))
            return false;

        // if there is an indent we need to start nested on a new line
        if (indent > 0)
        {
            builder.AppendLine().Append(' ', indent * 2);
        }
        
        var (obj, type, name) = var;

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
            builder.Append("?null?");
            return true;
        }
        
        
    }


    public string Dump<T>(T? value, [CallerArgumentExpression(nameof(value))] string? valueName = null)
    {
        var var = DumpVar.Create<T>(value, valueName);
        
    }
}

public record class DumpBox
{
    public static DumpBox Box<T>(T? value)
    {
        return new((object?)value, value?.GetType() ?? typeof(T));
    }
    
    public static DumpBox Box(object? value)
    {
        return new(value, value?.GetType() ?? typeof(object));
    }
    
    public static DumpBox Box(object? value, Type? type)
    {
        return new(value, type ?? value?.GetType() ?? typeof(object));
    }
    
    public object? Object { get; }
    
    public Type Type { get; }

    public bool ContainsNull => Object is null;
    
    protected DumpBox(object? obj, Type type)
    {
        this.Object = obj;
        this.Type = type;
    }

    public void Deconstruct(out object? o, out Type type)
    {
        o = Object;
        type = Type;
    }
}

public sealed record class DumpVar : DumpBox
{
    public static DumpVar Create<T>(T? value, [CallerArgumentExpression(nameof(value))] string? valueName = null)
    {
        return new DumpVar((object?)value, value?.GetType() ?? typeof(T), valueName);
    }

    public static DumpVar Create(object? obj, [CallerArgumentExpression(nameof(obj))] string? objectName = null)
    {
        return new DumpVar(obj, obj?.GetType() ?? typeof(object), objectName);
    }

    public static DumpVar Create(object? obj, Type? type, [CallerArgumentExpression(nameof(obj))] string? objectName = null)
    {
        return new DumpVar(obj, type ?? obj?.GetType() ?? typeof(object), objectName);
    }


    public string? Name { get; }
    
    private DumpVar(object? obj, Type type, string? name) : base(obj, type)
    {
        this.Name = name;
    }

    public void Deconstruct(out object? obj, out Type type, out string? name)
    {
        obj = Object;
        type = Type;
        name = Name;
    }
}