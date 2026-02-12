namespace ScrubJay.Enums;

[PublicAPI]
public sealed class EnumTypeInfo<E> : EnumTypeInfo
    where E : struct, Enum
{
    private EnumTypeInfo() : base(typeof(E)) { }

    public new IEnumerable<EnumMemberInfo<E>> Members => base.Members.OfType<EnumMemberInfo<E>>();

    public EnumMemberInfo<E>? GetMemberInfo(E mune)
    {
        return Members
            .FirstOrDefault(m => m.Member.IsEqual(mune));
    }
    
    public Option<EnumMemberInfo<E>> TryGetMemberInfo(E mune)
    {
        return _members
            .OfType<EnumMemberInfo<E>>()
            .FirstOrDefault(m => m.Member.IsEqual(mune))
            .IsNotNull();
    }
}