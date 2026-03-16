using System.Collections.Frozen;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;

namespace ScrubJay.Enums;

[PublicAPI]
public abstract class EnumMemberInfo
{
    protected readonly Enum _enumMember;
    protected readonly string _name;
    protected readonly FrozenDictionary<string, string?> _aliases;
    protected readonly Attribute[] _attributes;

    public Enum Member => _enumMember;

    protected EnumMemberInfo(FieldInfo memberField)
    {
        _enumMember = (Enum)memberField.GetValue(null)!;
        _name = memberField.Name;
        _attributes = Attribute.GetCustomAttributes(memberField);
        
        var aliases = new Dictionary<string, string?>();
        foreach (var attribute in _attributes)
        {
            if (attribute is DisplayAttribute displayAttribute)
            {
                aliases["Display.Name"] = displayAttribute.Name;
            }
            else if (attribute is EnumMemberAttribute enumMemberAttribute)
            {
                if (enumMemberAttribute.IsValueSetExplicitly)
                {
                    aliases["EnumMember.Value"] = enumMemberAttribute.Value;
                }
            }
            else if (attribute is DescriptionAttribute descriptionAttribute)
            {
                
            }
        }

    }

    public Result<Enum> TryParse(scoped text text, StringComparison comparison = StringComparison.Ordinal)
    {
        if (text.Equate(_name, comparison))
            return _enumMember;
        if (_aliases is not null)
        {
            foreach (var alias in _aliases)
            {
                if (text.Equate(alias, comparison))
                    return _enumMember;
            }
        }
        if (long.TryParse(text, NumberStyles.Any, null, out var i64))
        {
            if (i64 == _i64Value)
                return _enumMember;
        }
        return Ex.Parse(text, )
    }
}

public class EnumMemberInfo<E> : EnumMemberInfo
    where E : struct, Enum
{
    protected readonly E _enum;

    public new E Member => _enum;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long ToInt64() => _enum.ToInt64();
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong ToUInt64() => _enum.ToUInt64();
    
    public new Result<Enum> TryParse(scoped text text)
    {
        
    }
}