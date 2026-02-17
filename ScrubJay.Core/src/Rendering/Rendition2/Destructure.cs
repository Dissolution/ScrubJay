// using System.Linq.Expressions;
// using System.Reflection;
//
// namespace ScrubJay.ARCHIVE.Destructuring;
//
// [PublicAPI]
// public static partial class Destructure
// {
//     extension(TextBuilder builder)
//     {
//         public TextBuilder Destruct<T>(T? value)
//         {
//             if (value is Guid guid)
//             {
//                 return builder.DestructGuid(guid);
//             }
//             else if (value is MemberInfo member)
//             {
//                 return builder.DestructMember(member);
//             }
//             else if (value is Expression expression)
//             {
//                 return builder.DestructExpression(expression);
//             }
//             else if (value is Enum @enum)
//             {
//                 return builder.DestructEnum(@enum);
//             }
//             else
//             {
//                 return builder.Append<T>(value);
//             }
//         }
//
//       
//
//         public TextBuilder DestructGuid(Guid guid)
//         {
//             var buffer = builder.Allocate(36);
//             var formatted = guid.TryFormat(buffer, out int written, "D");
//             Guard.IsTrue(formatted);
//             Guard.IsEqual(written, 36);
//             buffer.ForEach((ref ch) =>
//             {
//                 if (ch >= 'a' && ch <= 'f')
//                     ch = (char)(ch - ('a' - 'A'));
//             });
//             return builder;
//         }
//     }
//
//
//     public static string Value<T>(T? value)
//     {
//         using var builder = new TextBuilder();
//         builder.Destruct<T>(value);
//         return builder.ToString();
//     }
// }