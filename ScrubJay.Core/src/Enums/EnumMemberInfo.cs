#pragma warning disable CA1819

#if !NETFRAMEWORK && !NETSTANDARD
using System.ComponentModel.DataAnnotations;
#endif
using System.Reflection;
using ScrubJay.Rendering.Rendition3;


namespace ScrubJay.Enums;

[PublicAPI]
public abstract class EnumMemberInfo :
#if NET7_0_OR_GREATER
    IEqualityOperators<EnumMemberInfo, EnumMemberInfo, bool>,
#endif
    IEquatable<EnumMemberInfo>,
    IRenderable
{
    public static bool operator ==(EnumMemberInfo? left, EnumMemberInfo? right)
        => Equate.Values(left, right);

    public static bool operator !=(EnumMemberInfo? left, EnumMemberInfo? right)
        => !Equate.Values(left, right);

    protected static void AddAlias(string? alias, HashSet<string> aliases, ref string render)
    {
        if (!string.IsNullOrWhiteSpace(alias))
        {
            if (aliases.Add(alias!))
            {
                render = alias!;
            }
        }
    }


    private readonly HashSet<string> _aliases = [];
    private readonly string _render;

    public EnumTypeInfo EnumTypeInfo { get; }
    public Attribute[] Attributes { get; }
    public string Name { get; }
    public string Description => _render;
    public IReadOnlyCollection<string> Aliases => _aliases;
    public object Member { get; }
    public long I64Value { get; }

    protected EnumMemberInfo(EnumTypeInfo enumTypeInfo, FieldInfo memberField)
    {
        Debug.Assert(enumTypeInfo is not null);
        Debug.Assert(memberField is not null);
        Debug.Assert(memberField!.IsStatic);
        Debug.Assert(memberField.DeclaringType == enumTypeInfo!.EnumType);

        EnumTypeInfo = enumTypeInfo;
        Attributes = Attribute.GetCustomAttributes(memberField);
        Name = memberField.Name;
        Member = memberField.GetValue(null).ThrowIfNull();
        I64Value = Convert.ToInt64(Member);

        // default render is name
        _render = Name;
        _aliases = new(StringComparer.Ordinal);

        // In order of least important to most (overwrite)
#if !NETFRAMEWORK && !NETSTANDARD
        
        var displayAttr = Attributes.OfType<DisplayAttribute>().FirstOrDefault();
        if (displayAttr is not null)
        {
            AddAlias(displayAttr.Name, _aliases, ref _render);
            AddAlias(displayAttr.ShortName, _aliases, ref _render);
            AddAlias(displayAttr.Description, _aliases, ref _render);
        }
#endif

        var descriptionAttr = Attributes.OfType<DescriptionAttribute>().FirstOrDefault();
        if (descriptionAttr is not null)
        {
            AddAlias(descriptionAttr.Description, _aliases, ref _render);
        }
      
        // shrink aliases to save memory
        _aliases.TrimExcess();
    }

    public bool HasAlias(string? str)
    {
        if (str is null)
            return false;

        if (str == Name)
            return true;

        return _aliases.Contains(str);
    }

    public bool Equals(EnumMemberInfo? enumMemberInfo)
    {
        return enumMemberInfo is not null &&
               enumMemberInfo.EnumTypeInfo == EnumTypeInfo &&
               enumMemberInfo.Name == Name;
    }

    public bool Equals(Enum e)
    {
        return e.GetType() == EnumTypeInfo.EnumType &&
               e.ToString() == Name;
    }

    public override bool Equals(object? obj)
    {
        if (obj is EnumMemberInfo enumMemberInfo)
            return Equals(enumMemberInfo);
        if (obj is Enum e)
            return Equals(e);
        return false;
    }

    public override int GetHashCode()
    {
        return Hasher.HashMany(EnumTypeInfo, Name);
    }

    public TextBuilder RenderTo(TextBuilder builder)
    {
        return builder.Append(_render);
    }

    public override string ToString() => Name;
}