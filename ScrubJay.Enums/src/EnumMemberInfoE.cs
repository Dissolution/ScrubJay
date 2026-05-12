using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;
#if !NETFRAMEWORK && !NETSTANDARD
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
#endif
#if NET8_0_OR_GREATER
using System.Collections.Frozen;
#endif

namespace ScrubJay.Enums;


public sealed class EnumMemberInfo<TEnum> : EnumMemberInfo
    where TEnum : struct, Enum
{
#if NET8_0_OR_GREATER
    private readonly FrozenDictionary<string, string> _aliases;
#elif NET6_0_OR_GREATER
    private readonly ImmutableDictionary<string, string> _aliases;
#else
    private readonly ReadOnlyDictionary<string, string> _aliases;
#endif

    public readonly string MemberName;
    public readonly TEnum Member;
    public readonly Attribute[] Attributes;

    public override Enum MemberEnum => (Enum)Member;

    internal EnumMemberInfo(FieldInfo memberField)
    {
        Member = (TEnum)memberField.GetValue(null)!;
        MemberName = memberField.Name;
        Attributes = Attribute.GetCustomAttributes(memberField);



        var aliases = new Dictionary<string, string>();
        foreach (var attribute in Attributes)
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