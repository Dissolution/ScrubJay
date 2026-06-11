//using ScrubJay.Text.Building;
//#pragma warning disable RCS1194, CA1032, CA1010
//
//namespace ScrubJay.Reflection.Exceptions;
//
//[PublicAPI]
//public class MemberException : ReflectionException
//{
//    public MemberInfo? Member { get; init; }
//
//    public MemberException()
//        : base()
//    {
//    }
//
//    public MemberException(MemberInfo? member)
//        : base()
//    {
//        Member = member;
//    }
//
//    public MemberException(MemberInfo? member, ref InterpolatedTextBuilder message)
//        : base(ref message)
//    {
//        Member = member;
//    }
//
//    public MemberException(MemberInfo? member, ref InterpolatedTextBuilder message, Exception? innerException)
//        : base(ref message, innerException)
//    {
//        Member = member;
//    }
//
//    public override string ToString()
//    {
//        return TextBuilder.Build($"{GetType():@} - {Member:@}: {Message}");
//    }
//}