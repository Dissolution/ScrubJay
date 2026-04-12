#if NET8_0_OR_GREATER
using System.Collections.Frozen;
#elif NET6_0_OR_GREATER
using System.Collections.Immutable;
#else
using System.Collections.ObjectModel;
#endif

#if NET6_0_OR_GREATER
using System.ComponentModel.DataAnnotations;
#endif

using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;

namespace ScrubJay.Enums;

[PublicAPI]
public abstract class EnumMemberInfo
{
    protected readonly Enum _enum;
    protected readonly string _name;
#if NET8_0_OR_GREATER
    protected readonly FrozenDictionary<string, string> _aliases;
#elif NET6_0_OR_GREATER
    protected readonly ImmutableDictionary<string, string> _aliases;
#else
    protected readonly ReadOnlyDictionary<string, string> _aliases;
#endif
    protected readonly Attribute[] _attributes;

    public Enum Enum => _enum;

    protected EnumMemberInfo(FieldInfo memberField)
    {
        _enum = (Enum)memberField.GetValue(null)!;
        _name = memberField.Name;
        _attributes = Attribute.GetCustomAttributes(memberField);

        var aliases = new Dictionary<string, string>();
        foreach (var attribute in _attributes)
        {
#if NET6_0_OR_GREATER
            if (attribute is DisplayAttribute displayAttribute)
            {
                addAlias("Display.Name", displayAttribute.Name);
                addAlias("Display.Description", displayAttribute.Description);
                addAlias("Display.ShortName", displayAttribute.ShortName);
                addAlias("Display", displayAttribute.Name ?? displayAttribute.Description ?? displayAttribute.ShortName);
            }
            else
#endif
            if (attribute is DescriptionAttribute descriptionAttribute)
            {
                addAlias("Description", descriptionAttribute.Description);
            }
            else if (attribute is EnumMemberAttribute enumMemberAttribute)
            {
                addAlias("EnumMember", enumMemberAttribute.Value);
            }
            else if (attribute is DataMemberAttribute dataMemberAttribute)
            {
                addAlias("DataMember", dataMemberAttribute.Name);
            }
        }

#if NETSTANDARD2_1 || NET6_0_OR_GREATER
        aliases.TrimExcess();
#endif
#if NET8_0_OR_GREATER
        _aliases = aliases.ToFrozenDictionary();
#elif NET6_0_OR_GREATER
        _aliases = aliases.ToImmutableDictionary();
#else
        _aliases = new(aliases);
#endif

        return;

        void addAlias(string name, string? alias)
        {
            if (alias is not null)
            {
                aliases[name] = alias;
            }
        }
    }
}

[PublicAPI]
public sealed class EnumMemberInfo<TEnum> : EnumMemberInfo
    where TEnum : struct, Enum
{
    internal readonly TEnum _tenum;

    public TEnum Member => _tenum;

    internal EnumMemberInfo(FieldInfo memberField) : base(memberField)
    {
        _tenum = (TEnum)memberField.GetValue(null)!;
    }
}