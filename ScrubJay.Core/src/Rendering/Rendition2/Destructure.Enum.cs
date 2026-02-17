// namespace ScrubJay.ARCHIVE.Destructuring;
//
// partial class Destructure
// {
//     extension(TextBuilder builder)
//     {
//         public TextBuilder DestructEnum<E>(E @enum)
//             where E : struct, Enum
//         {
//             var display = EnumMemberInfo.For<E>(@enum)!.Display;
//             return builder.Append(display);
//         }
//
//         public TextBuilder DestructEnum(Enum? @enum)
//         {
//             if (@enum is null)
//                 return builder;
//             var display = EnumMemberInfo.For(@enum)!.Display;
//             return builder.Append(display);
//         }
//     }
// }