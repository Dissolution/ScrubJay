// #if NET9_0_OR_GREATER
//
// using InlineIL;
// using static InlineIL.IL;
// // ReSharper disable MethodOverloadWithOptionalParameter
//
// namespace ScrubJay.ARCHIVE.Destructuring;
//
// partial class Destructure
// {
//     [MethodImpl(MethodImplOptions.AggressiveInlining)]
//     private static TextBuilder FastDestructureNonRefStruct<T>(TextBuilder builder, T value)
//         where T : allows ref struct // but _never_ will be
//     {
//         Emit.Ldarg(nameof(builder));
//         Emit.Ldarg(nameof(value));
//         Emit.Call(new MethodRef(typeof(Destructure), nameof(Destruct), 
//             typeof(TextBuilder),
//             TypeRef.MethodGenericParameters[0]
//             ).MakeGenericMethod(typeof(T)));
//         return Return<TextBuilder>();
//     }
//
//     public static TextBuilder DestructValue<T>(this TextBuilder builder, T? value,
//         TypeConstraints.AllowsRefStruct<T> _ = default)
//         where T : allows ref struct
//     {
//         if (value is null)
//         {
//             return builder;
//         }
//
//         var type = typeof(T);
//
//         // ref structs we just append ToString
//         if (type.IsByRefLike)
//         {
//             return builder.Append<T>(value, _);
//         }
//
//         Emit.Ldarg(nameof(builder));
//         Emit.Ldarg(nameof(value));
//         Emit.Call(new MethodRef(typeof(Destructure), nameof(FastDestructureNonRefStruct)).MakeGenericMethod(typeof(T)));
//         return Return<TextBuilder>();
//     }
//
//
//     public static string Value<T>(T? value, TypeConstraints.AllowsRefStruct<T> _ = default)
//         where T : allows ref struct
//     {
//         using var builder = new TextBuilder();
//         builder.DestructValue<T>(value, _);
//         return builder.ToString();
//     }
// }
//
// #endif