// #pragma warning disable CA1069
//
// namespace ScrubJay.ARCHIVE.Destructuring;
//
// [PublicAPI]
// [Flags]
// public enum DestructureOptions
// {
//     None = 0,
//     Visibility = 1 << 0,
//     Type = 1 << 1,
//     DeclaringType = 1 << 2,
//     Name = 1 << 3,
//     GenericTypes = 1 << 4,
//     Parameters = 1 << 5,
//     Access = 1 << 6,
//     
//     ForParameter = Type | Name | Parameters,
//     ForType = DeclaringType | Name | GenericTypes,
//     ForMethod = Visibility | DeclaringType | Type | Name | GenericTypes | Parameters,
//     ForConstructor = Visibility | Type | Name | Parameters,
//     ForField = Visibility | Type | Name,
//     ForProperty = Visibility | Type | Name | Parameters | Access,
//     ForEvent = Visibility | Type | Name,
//     ForExpression = Type | Name | GenericTypes | Parameters,
//     
//     Simple = Type | DeclaringType | Name | GenericTypes,
//     
//     All = Visibility | DeclaringType | Type | Name | Parameters | Access,
// }