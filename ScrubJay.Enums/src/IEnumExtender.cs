//using ScrubJay.Parsing;
//
//namespace ScrubJay.Enums.scratch_3;
//
//public interface IEnumExtender<TSelf, TEnum>
//    where TSelf : IEnumExtender<TSelf, TEnum>
//    where TEnum : struct, Enum
//{
//#if NET7_0_OR_GREATER
//    public static abstract TSelf Instance { get; }
//
//    public static abstract Result<TEnum, ParseException> TryParse(scoped ReadOnlySpan<char> text);
//
//    public static abstract string ToString(TEnum @enum);
//#endif
//}