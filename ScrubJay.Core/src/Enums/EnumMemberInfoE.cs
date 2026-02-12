using System.Reflection;

namespace ScrubJay.Enums;

[PublicAPI]
public sealed class EnumMemberInfo<E> : EnumMemberInfo
    where E : struct, Enum
{
    public new E Member { get; }

    private EnumMemberInfo(EnumTypeInfo<E> enumTypeInfo, FieldInfo memberField)
        : base(enumTypeInfo, memberField)
    {
        Member = memberField.GetValue(null).ThrowIfNot<E>();
    }
}