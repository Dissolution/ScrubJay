namespace ScrubJay.Reflection.Lightweight;

public static class MemberExtensions
{
    extension(MemberInfo? member)
    {
        [NotNullIfNotNull(nameof(member))]
        public Type? ParentType
        {
            get
            {
                if (member is null) return null;
                return member.DeclaringType ?? member.ReflectedType ?? member.Module.GetType();
            }
        }
    }
}