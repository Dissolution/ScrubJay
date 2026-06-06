using ScrubJay.Errors;
using ScrubJay.Reflection.Extensions;

namespace ScrubJay.Reflection.Validation;

public static class GuardExtensions
{
    extension(Guard)
    {
        public static M IsStatic<M>([AllowNull, NotNull] M? member,
            [CallerArgumentExpression(nameof(member))]
            string? memberName = null)
            where M : MemberInfo
        {
            if (member is null)
                throw Ex.ArgNull(member, memberName);
            if (!member.IsStatic)
                throw Ex.Arg(member, "is not static", memberName);
            return member;
        }

        public static M IsNotStatic<M>([AllowNull, NotNull] M? member,
            [CallerArgumentExpression(nameof(member))]
            string? memberName = null)
            where M : MemberInfo
        {
            if (member is null)
                throw Ex.ArgNull(member, memberName);
            if (member.IsStatic)
                throw Ex.Arg(member, "is static", memberName);
            return member;
        }
    }
}