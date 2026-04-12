using ScrubJay.Enums.SourceGen.Utilities;

namespace ScrubJay.Enums.SourceGen.Models;

public readonly record struct EnumMemberToExtend
{
    public readonly string Name;
    public readonly object Value;
    public readonly SGArray<AttributeDefinition> Attributes;

    public EnumMemberToExtend(string name, object value, SGArray<AttributeDefinition> attributes)
    {
        Name = name;
        Value = value;
        Attributes = attributes;
    }
}