using System.Reflection;

namespace ScrubJay.Rendering.Rendition5;

[PublicAPI]
public static class MethodRenderer
{
    [RenderToMethod]
    public static void RenderMethodTo(MethodBase method, TextBuilder builder)
    {
        builder.Render(method.ParentType).Append('.')
            .If(method, static m => m.IsGeneric,
                static (tb, m) =>
                {
                    int i = m.Name.IndexOf('`');
                    if (i >= 0)
                    {
                        tb.Append(m.Name.AsSpan(0, i));
                    }
                    else
                    {
                        tb.Append(m.Name);
                    }
            
                    tb.Append('<')
                        .Delimit(", ", method.GetGenericArguments(), TBA<Type>.Render)
                        .Append('>');
                })

            .Append('(')
            .Delimit(", ", method.GetParameters(), "@")
            .Append(')')
            .If(method, static m => m.Is<MethodInfo>())
        
        
        if (method is MethodInfo methodInfo)
        {
            builder.Append(" -> ").Render(methodInfo.ReturnType);
        }
    }
}