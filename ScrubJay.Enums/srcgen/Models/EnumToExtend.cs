using ScrubJay.Enums.SourceGen.Utilities;

namespace ScrubJay.Enums.SourceGen.Models;

public readonly record struct EnumToExtend
{
    public readonly string TypeNamespace;
    public readonly string TypeName;
    public readonly Type UnderlyingType;
    public readonly SGArray<AttributeDefinition> Attributes;
    public readonly SGArray<EnumMemberToExtend> Members;

    public string TypeFullName => $"{TypeNamespace}.{TypeName}";
    
    public bool HasFlagsAttribute
    {
        get
        {
            return Attributes.Any(attr => attr.TypeFullName == "System.FlagsAttribute");
        }
    }

    public bool IsUnsigned
    {
        get
        {
            return Type.GetTypeCode(UnderlyingType) is TypeCode.Byte or TypeCode.UInt16 or  TypeCode.UInt32 or TypeCode.UInt64;
        }
    }

    public EnumToExtend(string typeNamespace, string typeName, Type underlyingType, SGArray<AttributeDefinition> attributes, SGArray<EnumMemberToExtend> members)
    {
        TypeNamespace = typeNamespace;
        TypeName = typeName;
        UnderlyingType = underlyingType;
        Attributes = attributes;
        Members = members;
    }

    public void Deconstruct(out string typeNamespace, out string typeName, out Type underlyingType, out SGArray<AttributeDefinition> attributes, out SGArray<EnumMemberToExtend> members)
    {
        typeNamespace = TypeNamespace;
        typeName = TypeName;
        underlyingType = UnderlyingType;
        attributes = Attributes;
        members = Members;
    }
}