// #if NET8_0_OR_GREATER
// using System.Collections.Frozen;
// #endif
// using System.Reflection;
// using InlineIL;
// using static InlineIL.IL;
//
// namespace ScrubJay.Rendering.Rendition4;
//
// [PublicAPI]
// public interface IRenderable
// {
//     void RenderTo(TextBuilder builder);
// }
//
// [PublicAPI]
// public interface IRenderer;
//
//
// public delegate void RenderTo<in T>(T value, TextBuilder builder);
//
// public sealed class RenderableRenderer : IRenderer
// {
//     public static void RenderTo<R>(R renderable, TextBuilder builder)
//         where R : IRenderable
//     {
//         throw new NotImplementedException();
//     }
// }
//
// public static class RendererCache
// {
//     private readonly List<Delegate> _renderToDelegates;
//     private readonly TypeMap<Delegate> _cache = [];
//
//     static RendererCache()
//     {
//         AppDomain
//             .CurrentDomain
//             .GetAssemblies()
//             .SelectMany(static assembly => Try(assembly.GetTypes).OkOr([]))
//             .Where(static type => type.IsClass && type.IsSealed && type.IsAssignableTo<IRenderer>())
//             .SelectMany(static type => type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly))
//             .Where(static method => method.Name == "RenderTo" && method.ReturnType == typeof(void) && method.GetParameters().Length ==2 && method.GetParameters()[1].ParameterType == typeof(TextBuilder))
//             .Select(static method => Delegate.CreateDelegate(typeof(RenderTo<>).MakeGenericType())
//         Debugger.Break();
//     }
//
//     private static IRenderer? FindRenderer<T>()
// #if NET9_0_OR_GREATER
//         where T : allows ref struct
// #endif
//     {
//         return _cache.GetOrAdd<T>(static () =>
//         {
//             using var e = _renderers.GetEnumerator();
//             while (e.MoveNext())
//             {
//                 if (e.Current.CanRender<T>())
//                 {
//                     return e.Current;
//                 }
//             }
//
//             return null;
//         });
//     }
//
//
//     public static TextBuilder RenderTo<T>(this T? value, TextBuilder builder)
// #if NET9_0_OR_GREATER
//         where T : allows ref struct
// #endif
//     {
//         if (value is null)
//         {
//             return builder.Append("〈null〉");
//         }
//
//         var renderer = FindRenderer<T>();
//         if (renderer is not null)
//         {
//             renderer.RenderTo<T>(value, builder);
//             return builder;
//         }
//
//         return builder.Append<T>(value);
//     }
//
//     public static string Render<T>(this T? value)
// #if NET9_0_OR_GREATER
//         where T : allows ref struct
// #endif
//     {
//         using var builder = TextBuilder.New;
//         value.RenderTo<T>(builder);
//         return builder.ToString();
//     }
// }