using System.Reflection;

namespace ScrubJay.Text.Rendering;

[PublicAPI]
public static class ReflectionRenderers
{
    internal static void WriteNameAndGenericTypes(
        TextBuilder builder,
        string? memberName,
        Type[]? genericTypes)
    {
        if (memberName is not null)
        {
            int i = memberName.LastIndexOf('`');
            if (i >= 0)
            {
                builder.Write(memberName.AsSpan(0, i));
            }
            else
            {
                builder.Write(memberName);
            }
        }

        //string sep = type.IsGenericTypeDefinition ? "," : ", ";
        string sep = ", ";

        if (!genericTypes.IsNullOrEmpty())
        {
            builder
                .Append('<')
                .Delimit(sep, genericTypes, TB.Render)
                .Append('>');
        }
    }

    [RenderToMethod]
    public static void RenderMemberTo(MemberInfo member, TextBuilder builder)
    {
        switch (member)
        {
            case EventInfo eventInfo:
                break;
            case ConstructorInfo constructorInfo:
                break;
            case FieldInfo fieldInfo:
                break;
            case MethodInfo methodInfo:
                break;
            case PropertyInfo propertyInfo:
                break;
            case Type type:
            {
                TypeRenderer.RenderTypeTo(type, builder);
                return;
            }
            default:
                break;
        }
        
        // nothing
        Debugger.Break();
        return;
    }

    [RenderToMethod]
    public static void RenderParameterTo(ParameterInfo parameter, TextBuilder builder)
    {
        builder.Append(parameter.Name ?? "_")
            .Append(": ")
            .Render(parameter.ParameterType)
            .If(parameter, static p => p.HasDefaultValue, (tb, p) => tb.Append(" = ").Render(p.DefaultValue));
    }

//    
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

//    }
}