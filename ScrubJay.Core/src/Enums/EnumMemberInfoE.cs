using System.Reflection;

namespace ScrubJay.Enums;

[PublicAPI]
public class EnumMemberInfo<E> : EnumMemberInfo
    where E : struct, Enum
{
    public new E Member { get; }

    protected EnumMemberInfo(EnumTypeInfo<E> enumTypeInfo, FieldInfo memberField)
        : base(enumTypeInfo, memberField)
    {
        Member = memberField.GetValue(null).ThrowIfNot<E>();
    }
}

