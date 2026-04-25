//using System.Reflection;
//
//namespace ScrubJay.Rendering.Rendition5;
//
//[PublicAPI]
//public static class ReflectionRenderers
//{
//
//    [RenderToMethod]
//    public static void RenderMethodTo(MethodBase method, TextBuilder builder)
//    {
//        builder.Render(method.ParentType).Append('.')
//            .If(method, static m => m.IsGeneric,
//                static (tb, m) =>
//                {
//                    int i = m.Name.IndexOf('`');
//                    if (i >= 0)
//                    {
//                        tb.Append(m.Name.AsSpan(0, i));
//                    }
//                    else
//                    {
//                        tb.Append(m.Name);
//                    }
//
//                    tb.Append('<')
//                        .Delimit(", ", m.GetGenericArguments(), TBA<Type>.Render)
//                        .Append('>');
//                })
//
//            .Append('(')
//            .Delimit(", ", method.GetParameters(), "@")
//            .Append(')')
//            .If(method, static m => m.Is<MethodInfo>(), static (tb, m) => tb.Render(m.ReturnType));
//    }
//
//    [RenderToMethod<ParameterInfo>]
//    public static void RenderParameterTo(ParameterInfo parameter, TextBuilder builder)
//    {
//        builder.Append(parameter.Name ?? "〈?〉")
//            .Append(": ")
//            .Render(parameter.ParameterType)
//            .If(parameter, static p => p.HasDefaultValue, static (tb, p) => tb.Render(p.DefaultValue));
//    }
//}