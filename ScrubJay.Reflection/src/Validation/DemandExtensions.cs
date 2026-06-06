using ScrubJay.Errors;
using ScrubJay.Errors.Utilities;
using ScrubJay.Errors.Validation;
using ScrubJay.Reflection.Extensions;

namespace ScrubJay.Reflection.Validation;

public static class DemandExtensions
{
    extension(Demand)
    {
        public static void Static<M>(
            [AllowNull, NotNull] M? member,
            [CallerArgumentExpression(nameof(member))]
            string? memberName = null)
            where M : MemberInfo
        {
            Demand.NotNull(member, null, memberName);
            if (!member.IsStatic)
                Throw.Arg(member, "was not static", memberName);
        }

        public static void NotStatic<M>(
            [AllowNull, NotNull] M? member,
            string? info = null,
            [CallerArgumentExpression(nameof(member))]
            string? memberName = null)
            where M : MemberInfo
        {
            Demand.NotNull(member, null, memberName);
            if (member.IsStatic)
                Throw.Arg(member, "was static", memberName);
        }
    }
}