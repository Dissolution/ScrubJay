using System.Reflection;

namespace ScrubJay.Text.Extensions;

[PublicAPI]
public static class ReflectionExtensions
{
    extension(MemberInfo? member)
    {
        public Type? ParentType
        {
            [return: NotNullIfNotNull(nameof(member))]
            get
            {
                if (member is not null)
                {
                    if (member.DeclaringType is not null)
                        return member.DeclaringType;
                    if (member.ReflectedType is not null)
                        return member.ReflectedType;
                    return member.Module.GetType();
                }
                return null;
            }
        }
    }
}